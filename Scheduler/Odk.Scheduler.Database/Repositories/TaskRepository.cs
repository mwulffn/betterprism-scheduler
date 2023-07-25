using Odk.Scheduler.Database.Models;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database.Repositories
{
    class TaskRepository : Repository<Guid, Task>, ITaskRepository
    {
        private readonly IWorkblockRepository workblockRepository;

        public TaskRepository(IDatabase database, IWorkblockRepository workblockRepository) : base(database)
        {
            this.workblockRepository = workblockRepository;
        }

        public Session SessionFromTask(Task task, bool masterTask = true)
        {
            var session = new Session()
            {
                SessionId = Guid.NewGuid(),
                TaskId = task.TaskId,
                State = SessionState.Ready,
                DelayStateTransition = DateTime.Now.AddSeconds(-2),
                BPSessionId = null,
                AllocatedResource = null,
                Master = masterTask,
                Launch = null,
                Run = null,
                Complete = null,
                Fail = null,
                Created = DateTime.Now
            };

            if (task.Launch.HasValue)
            {
                var wb = workblockRepository.Single(task.Launch.Value);
                session.Launch = wb.ProcessId;
                session.LaunchParameters = wb.Parameters ?? "";
                session.LaunchPcd = wb.PostCompletionDelay;
            }

            if (task.Run.HasValue)
            {
                var wb = workblockRepository.Single(task.Run.Value);
                session.Run = wb.ProcessId;
                session.RunParameters = wb.Parameters ?? "";
                session.RunPcd = wb.PostCompletionDelay;
            }

            if (task.Complete.HasValue)
            {
                var wb = workblockRepository.Single(task.Complete.Value);
                session.Complete = wb.ProcessId;
                session.CompleteParameters = wb.Parameters ?? "";
                session.CompletePcd = wb.PostCompletionDelay;
            }

            if (task.Fail.HasValue)
            {
                var wb = workblockRepository.Single(task.Fail.Value);
                session.Fail = wb.ProcessId;
                session.FailParameters = wb.Parameters ?? "";
                session.FailPcd = wb.PostCompletionDelay;
            }

            return session;
        }

        public IEnumerable<Task> EnabledTasks()
        {
            return database.Fetch<Task>("WHERE enabled = 1 AND Deleted = 0 ORDER BY [Name] ASC");
        }

        public IEnumerable<Task> AllTasks()
        {
            return database.Fetch<Task>("WHERE 1 = 1 AND Deleted = 0 ORDER BY [Name] ASC");
        }

        public IEnumerable<AtDate> AllAtDates(Guid taskId)
        {
            return database.Fetch<AtDate>("WHERE TaskId=@0 ORDER BY AtDate ASC;", taskId);
        }

        public IEnumerable<AtDate> ActiveAtDates(Guid taskId)
        {
            return database.Fetch<AtDate>("WHERE HasBeenTriggered = 0 AND TaskId=@0 ORDER BY AtDate ASC;", taskId);
        }

        public void RegisterTrigger(AtDate date)
        {
            date.HasBeenTriggered = true;
            database.Save(date);
        }

        public AtDate AddAtDate(AtDate date)
        {
            if (date.At < DateTime.Now)
                return null;
                        
            var id = (int)database.Insert(date);
            date.Id = id;

            return date;
        }

        public void DeleteAtDate(int atDateId)
        {
            database.Delete<AtDate>(atDateId);
        }
    }
}