using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/task")]
    public class TaskController : BaseController<Guid, Task>
    {
        private readonly ITaskRepository taskRepository;
        private readonly ISessionRepository sessionRepository;

        public TaskController(IBluePrism bluePrism, ITaskRepository taskRepository, ISessionRepository sessionRepository) : base(bluePrism)
        {
            this.taskRepository = taskRepository;
            this.sessionRepository = sessionRepository;
        }

        public override IEnumerable<Task> Get()
        {
            return taskRepository.AllTasks();
        }

        public override Task Get(Guid id)
        {
            var task = taskRepository.SingleOrDefault(id);

            if (task == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return task;
        }

        public override Task Post([FromBody] Task obj)
        {
            obj.TaskId = Guid.NewGuid();
            taskRepository.Insert(obj);

            return obj;
        }

        public override Task Put(Guid id, [FromBody] Task obj)
        {
            var task = taskRepository.SingleOrDefault(id);

            if (task == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            task.Name = !string.IsNullOrEmpty(obj.Name) ? obj.Name : task.Name ?? "Unknown";
            task.Trigger = obj.Trigger;
            task.Cron = obj.Cron;
            task.ScaleLimit = obj.ScaleLimit > 0 ? obj.ScaleLimit : 1;
            task.ScaleThreshold = obj.ScaleThreshold > 0 ? obj.ScaleThreshold : 1;
            task.Complete = obj.Complete;
            task.Launch = obj.Launch;
            task.Run = obj.Run;
            task.Fail = obj.Fail;
            task.Enabled = obj.Enabled;
            task.Workqueue = obj.Workqueue;
            taskRepository.Save(task);

            return task;
        }

        public override Task Delete(Guid id)
        {
            var task = taskRepository.Single(id);
            task.Deleted = true;
            taskRepository.Save(task);

            return task;
        }

        [HttpPost]
        [Route("{id}/dispatch")]
        public bool Dispatch(Guid id, int delay = 0)
        {
            delay = Math.Max(0, delay);
            var task = taskRepository.Single(id);

            if (sessionRepository.SessionExistsForTask(task))
                return false;

            var session = taskRepository.SessionFromTask(task, true);
            session.DelayStateTransition = DateTime.Now.AddHours(delay);
            sessionRepository.Insert(session);
            logger.Info($"Instant dispatching a '{task.Name}' session. (master)");
            task.LastTriggered = DateTime.Now;
            taskRepository.Save(task);

            return true;
        }

        [HttpGet]
        [Route("{id}/atdates")]
        public IEnumerable<AtDate> AtDates(Guid id)
        {
            return taskRepository.ActiveAtDates(id);
        }

        [HttpPost]
        [Route("{id}/addatdate")]
        public AtDate AddAtDate(AtDate date)
        {
            if (date.At < DateTime.Now)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            return taskRepository.AddAtDate(date);
        }

        [HttpDelete]
        [Route("{id}/deleteatdate/{dateId}")]
        public void DeleteAtDate(int dateId)
        {
            taskRepository.DeleteAtDate(dateId);
        }

        [HttpPut]
        [Route("{id}/toggleenabled")]
        public bool ToggleEnabled(Guid id)
        {
            var task = taskRepository.Single(id);
            task.Enabled = !task.Enabled;
            taskRepository.Save(task);

            return task.Enabled;
        }

        [HttpPut]
        [Route("{id}/setenabled")]
        public bool SetEnabled(Guid id, bool enabled)
        {
            var task = taskRepository.Single(id);
            task.Enabled = enabled;
            taskRepository.Save(task);

            return task.Enabled;
        }
    }
}