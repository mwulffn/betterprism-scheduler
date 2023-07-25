using System;

namespace odk.Scheduler.DB
{
    public class AtDate
    {
        public Guid Id { get; set; }
        public DateTime RunAt { get; set; }

        public virtual Task Task { get; set; }
    }
}
