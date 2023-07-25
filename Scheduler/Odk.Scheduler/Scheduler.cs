using Microsoft.Owin.Hosting;
using NCrontab;
using System;
using System.Linq;
using System.Timers;
using System.Configuration;
using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;

namespace Odk.Scheduler
{
    public class Scheduler
    {
        public DateTime LastScheduling { get; set; }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        readonly Timer _timer;
        private readonly IBluePrism _bluePrism;
        private readonly ITaskRepository taskRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly IWorkblockRepository workblockRepository;
        private IDisposable _webApp;

        public Scheduler(IBluePrism bluePrism, ITaskRepository taskRepository, ISessionRepository sessionRepository, IWorkblockRepository workblockRepository)
        {
            _timer = new Timer(10000) { AutoReset = true };
            _timer.Elapsed += Heartbeat;
            LastScheduling = DateTime.MinValue;
            _bluePrism = bluePrism;

            this.taskRepository = taskRepository;
            this.sessionRepository = sessionRepository;
            this.workblockRepository = workblockRepository;

            Logger.Info("Constructing the scheduler");
            Logger.Info($"Running with {int.Parse(ConfigurationManager.AppSettings["Licenses"] ?? "1")} licenses activated");
        }

        private void Heartbeat(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            try
            {
                if ((DateTime.Now - LastScheduling).TotalSeconds >= 60)
                    Schedule();
            }
            catch (Exception err)
            {
                Logger.Error(err, "Error in schedule");
            }

            try
            {
                Dispatch();
            }
            catch (Exception err)
            {
                Logger.Error(err, "Error in dispatch");
            }

            _timer.Start();
        }

        private void Schedule()
        {
            var now = DateTime.Now.Truncate(TimeSpan.TicksPerMinute);
            
            //This checks if something needs to be added to a session
            Logger.Trace("Schedule");
            LastScheduling = now;

            var tasks = taskRepository.EnabledTasks();

            foreach (var task in tasks)
            {
                switch (task.Trigger)
                {
                    case Trigger.Cron:
                        try
                        {
                            var s = CrontabSchedule.Parse(task.Cron);
                            var occurrence = s.GetNextOccurrence(now.AddMinutes(-1));

                            if (occurrence == now)
                            {
                                AddTaskToSession(task, true);
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e, "Error in task: " + task.Name);
                        }

                        break;
                    case Trigger.At:
                        var date = taskRepository.ActiveAtDates(task.TaskId).SingleOrDefault(a => a.At == now);

                        if (date == null)
                            break;

                        AddTaskToSession(task, true);

                        taskRepository.RegisterTrigger(date);
                        break;
                    case Trigger.Queue:                        
                        var workqueue = _bluePrism.ListWorkqueues().SingleOrDefault(a => a.Id == task.Workqueue);

                        if (workqueue == null)
                        {
                            Logger.Info($"{task.Workqueue} is unknown, it might have been renamed");
                            continue;
                        }

                        if (_bluePrism.PendingItems(workqueue) > 0)
                            AddTaskToSession(task, true);
                        
                        break;
                }
            }
        }

        private Session AddTaskToSession(Task task, bool masterTask)
        {
            if (masterTask && sessionRepository.SessionExistsForTask(task))
            {
                Logger.Debug($"Already running {task.Name} - ignoring trigger");
                return null;
            }

            Logger.Info($"Creating a new '{task.Name}' session. {(masterTask ? "(master)" : "(slave)")}");
            var session = taskRepository.SessionFromTask(task, masterTask);
            sessionRepository.Insert(session);
            task.LastTriggered = DateTime.Now;
            taskRepository.Save(task);

            return session;
        }

        private void Dispatch()
        {
            //This routine dispatches new tasks and updates running tasks.
            Logger.Trace("Dispatching");
            UpdateStatus();
            PromoteSlaves();
            AutoscaleSessions();
            DescaleSessions();
        }

        private void UpdateStatus()
        {
            //Update status
            var sessions = sessionRepository.ActiveSessions();
            var now = DateTime.Now;

            foreach (var session in sessions)
            {
                if (session.DelayStateTransition > now)
                    continue;

                switch (session.State)
                {
                    case SessionState.Ready:
                        if (!_bluePrism.HasAvailableResources())
                            continue;

                        session.NextState();
                        var resource = _bluePrism.AllocateResource();
                        session.AllocatedResource = resource.Id;
                        session.Dispatched = DateTime.Now;
                        sessionRepository.Save(session);
                        break;

                    case SessionState.Launching:
                    case SessionState.Running:
                    case SessionState.Completing:
                    case SessionState.Failing:
                        //In case of the autoscaler we may be running but not launched yet.
                        if (!session.BPSessionId.HasValue)
                            break;

                        var bpStatus = _bluePrism.SessionStatus(session.BPSessionId.Value);

                        switch (bpStatus)
                        {
                            case BPSessionStatus.Pending:
                                break;
                            case BPSessionStatus.Running:
                                break;
                            case BPSessionStatus.Stopping:
                                session.StopRequested = true;
                                sessionRepository.Save(session);
                                break;
                            case BPSessionStatus.Completed:
                            case BPSessionStatus.Stopped:
                                session.DelayStateTransition = DateTime.Now.AddSeconds(session.PostCompletionDelayForState());

                                session.NextState();
                                session.BPSessionId = null;
                                sessionRepository.Save(session);
                                break;
                            case BPSessionStatus.Terminated:
                                /* This needs to be smarter eventually */
                                var incident = new SessionIncident()
                                {
                                    SessionIncidentId = Guid.NewGuid(),
                                    SessionId = session.SessionId,
                                    BPResourceId = session.AllocatedResource.Value,
                                    BPSessionId = session.BPSessionId.Value,
                                    Created = DateTime.Now,
                                    Closed = null,
                                    Resolution = IncidentResolution.Unresolved
                                };
                                sessionRepository.AddSessionIncident(incident);

                                session.FailSession();
                                session.BPSessionId = null;
                                sessionRepository.Save(session);
                                break;
                        }
                        break;
                    case SessionState.Completed:
                    case SessionState.Failed:
                        var unusedResource = _bluePrism.GetResource(session.AllocatedResource.Value);
                        _bluePrism.FreeResource(unusedResource);

                        session.AllocatedResource = null;
                        session.BPSessionId = null;
                        session.Closed = DateTime.Now;
                        sessionRepository.Save(session);
                        break;

                    default:
                        break;
                }

                // we are ready to launch next stage?
                if (session.IsInARunnableState() && session.BPSessionId == null && session.AllocatedResource.HasValue && session.DelayStateTransition <= now)
                {
                    var process = _bluePrism.GetProcess(session.ProcessIdForState());
                    var resource = _bluePrism.GetResource(session.AllocatedResource.Value);
                    session.BPSessionId = _bluePrism.LaunchSession(process, resource, session.ParametersForState());
                    sessionRepository.Save(session);
                }
            }
        }

        void PromoteSlaves()
        {
            var slaveSessions = sessionRepository.ActiveSessions().Where(a => a.Master == false);

            foreach (var slave in slaveSessions)
            {
                var hasMaster = sessionRepository.ActiveSessions().Any(a => a.TaskId == slave.TaskId && a.Master);

                if (hasMaster)
                    continue;

                // Promote the current slave
                slave.Master = true;
                sessionRepository.Save(slave);
            }
        }

        // Scales back slaves if there are pending masters
        void DescaleSessions()
        {
            if (_bluePrism.HasAvailableResources())
                return;

            var sessions = sessionRepository.ActiveSessions();

            //Do we have any masters that can't run, due to excessive slaves
            if (!sessions.Any(a => a.Master && a.State == SessionState.Ready && !a.AllocatedResource.HasValue))
                return;

            //Is there a session being stopped right now? or if someone is about to quit. The session may also be completed but still in PCD
            if (sessions.Any(a => a.StopRequested || (a.State == SessionState.Completing || a.State == SessionState.Failing || a.State == SessionState.Completed || a.State == SessionState.Failed)))
                return;

            var slave = sessions.Where(a => !a.Master && a.State == SessionState.Running && a.BPSessionId.HasValue && a.AllocatedResource.HasValue).FirstOrDefault();

            if (slave == null)
                return;

            _bluePrism.RequestStop(slave.BPSessionId.Value);

            slave.StopRequested = true;
            sessionRepository.Save(slave);
        }

        void AutoscaleSessions()
        {
            // Sanity check that we have no ready masters.
            if (sessionRepository.ActiveSessions().Any(a => a.State == SessionState.Ready && a.Master))
                return;

            var sessions = sessionRepository.ActiveSessions().Where(a => a.BPSessionId.HasValue && a.AllocatedResource.HasValue && a.Master);

            foreach (var session in sessions)
            {
                if (!_bluePrism.HasAvailableResources())
                    return;

                if (!session.TaskId.HasValue)
                    continue;

                var task = taskRepository.SingleOrDefault(session.TaskId.Value);

                if (task == null || task.Trigger != Trigger.Queue || !task.Enabled)
                    continue;

                var workqueue = _bluePrism.GetWorkqueue(task.Workqueue.Value);

                if (_bluePrism.PendingItems(workqueue) < task.ScaleThreshold)
                    continue;

                var slaves = sessionRepository.ActiveSessions().Where(a => a.TaskId == task.TaskId && a.Master == false).Count();

                if (slaves >= task.ScaleLimit - 1) //The master task counts as an instance as well.
                    continue;

                var slaveSession = AddTaskToSession(task, false);

                slaveSession.NextState();
                var resource = _bluePrism.AllocateResource();
                slaveSession.AllocatedResource = resource.Id;
                slaveSession.Dispatched = DateTime.Now;
                sessionRepository.Save(slaveSession);
            }
        }

        public void Start()
        {
            string baseUrl = ConfigurationManager.AppSettings["Host"] ?? "http://localhost:9000/";
            _webApp = WebApp.Start<Startup>(url: baseUrl);
            Logger.Debug($"Rest api listening on: {baseUrl}");
            _timer.Start();
            Logger.Info("Timer started");
        }
        public void Stop()
        {
            _webApp.Dispose();
            _timer.Stop();
            Logger.Info("Timer stopped");
        }
    }
}