using System;

namespace Odk.Scheduler.Dto
{
    public class WorkqueueInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Running { get; set; }    
        public int Pending { get; set; }
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public int ScaleLimit { get; set; }
    }
}