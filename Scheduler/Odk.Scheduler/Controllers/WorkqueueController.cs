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
    public class WorkqueueController : BaseController<Guid, BPWorkqueue>
    {
        private readonly ITaskRepository taskRepository;

        public WorkqueueController(IBluePrism bluePrism, ITaskRepository taskRepository) : base(bluePrism)
        {
            this.taskRepository = taskRepository;
        }

        public override IEnumerable<BPWorkqueue> Get()
        {
            return bluePrism.ListWorkqueues();
        }

        [HttpGet]
        [Route("api/Workqueue/ActiveQueues")]
        public IEnumerable<WorkqueueInfo> ActiveQueues()
        {
            var tasks = taskRepository.EnabledTasks().Where(a => a.Trigger == Trigger.Queue && a.Workqueue.HasValue);
            var result = new List<WorkqueueInfo>();

            foreach (var task in tasks)
            {
                var bpqueue = bluePrism.GetWorkqueue(task.Workqueue.Value);
                var pending = bluePrism.PendingItems(bpqueue, true);
                var item = new WorkqueueInfo()
                {
                    Id = bpqueue.Id,
                    Name = bpqueue.Name,
                    Running = bpqueue.Running,
                    Pending = pending,

                    TaskId = task.TaskId,
                    TaskName = task.Name,
                    ScaleLimit = task.ScaleLimit
                };

                result.Add(item);
            }

            return result;
        }
    }
}