using System;

namespace odk.Scheduler.DB
{
    public class TaskWorkblock
    {
        public Guid Id { get; set; }

        public string Parameters { get; set; }

        public virtual Task Task { get; set; }
        public virtual Workblock Workblock { get; set; }
    }
}
