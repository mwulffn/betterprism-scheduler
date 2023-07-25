using System;

namespace Odk.Scheduler.Dto
{
    public class TaskOccurence
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public DateTime ExecutionTime { get; set; }        
    }
}