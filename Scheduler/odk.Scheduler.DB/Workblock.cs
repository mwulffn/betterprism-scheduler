using System;
using System.Collections.Generic;

namespace odk.Scheduler.DB
{
    public class Workblock
    {
        public Guid Id { get; set; }
        public Guid ProcessId { get; set; }

        public string Name { get; set; }

        public TaskState Intention { get; set; }
        public string DefaultParameters { get; set; }
        public int PostCompletionDelay { get; set; }


        public virtual ICollection<TaskWorkblock> TaskWorkblocks { get; set; }
    }
}
