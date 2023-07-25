
using NCrontab;
using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using Odk.Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : BaseController<int, DashboardItem>
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITaskRepository taskRepository;

        public DashboardController(IBluePrism bluePrism, ISessionRepository sessionRepository, ITaskRepository taskRepository) : base(bluePrism)
        {
            this.sessionRepository = sessionRepository;
            this.taskRepository = taskRepository;
        }

        public override IEnumerable<DashboardItem> Get()
        {
            var sessions = sessionRepository.ActiveSessions().Select(a => new DashboardItem(a)).ToList();

            foreach (var session in sessions)
            {
                if (session.ProcessId != Guid.Empty)
                    session.ProcessName = bluePrism.GetProcess(session.ProcessId)?.Name ?? "Unknown process";

                if (session.ResourceId != Guid.Empty)
                    session.ResourceName = bluePrism.GetResource(session.ResourceId)?.Name ?? "Unknown resource";

                if (session.TaskId.HasValue)
                    session.TaskName = taskRepository.Single(session.TaskId.Value)?.Name ?? "Task has been deleted";
            }

            return sessions;
        }

        [HttpGet]
        [Route("UpNext")]
        public IEnumerable<TaskOccurence> UpNext()
        {
            var tasks = taskRepository.EnabledTasks();
            var start = DateTime.Now;
            var stop = start.AddDays(1);
            var occurences = new List<TaskOccurence>();

            foreach (var task in tasks)
            {
                switch (task.Trigger)
                {
                    case Trigger.Cron:
                        {
                            var cron = CrontabSchedule.Parse(task.Cron);
                            var occ = cron.GetNextOccurrences(start, stop);

                            occurences.AddRange(occ.Select(a => new TaskOccurence()
                            {
                                TaskId = task.TaskId,
                                TaskName = task.Name,
                                ExecutionTime = a
                            }));

                            break;
                        }
                    case Trigger.At:
                        {
                            var dates = taskRepository.ActiveAtDates(task.TaskId).Where(a => a.At >= start && a.At <= stop);

                            occurences.AddRange(dates.Select(d => new TaskOccurence()
                            {
                                TaskId = task.TaskId,
                                TaskName = task.Name,
                                ExecutionTime = d.At
                            }));
                        }
                        break;
                }
            }

            return occurences.OrderBy(a => a.ExecutionTime).ThenBy(a=> a.TaskName);
        }
    }
}