using System;

namespace odk.Scheduler.DB
{
    public class ActivityLog
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public String Message { get; set; }
    }
}
