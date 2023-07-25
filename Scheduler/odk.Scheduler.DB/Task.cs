using System;
using System.Collections.Generic;

namespace odk.Scheduler.DB
{
    public class Task
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public Trigger Trigger { get; set; }
        public string Cron { get; set; }
        public string Workqueue { get; set; }

        public int ScaleLimit { get; set; }
        public int ScaleThreshold { get; set; }


        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        //References
        public virtual ICollection<AtDate> AtDates { get; set; }

        public virtual ICollection<TaskWorkblock> TaskWorkblocks { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }


        public string TriggerName
        {
            get
            {
                switch (Trigger)
                {
                    case Trigger.Disabled:
                        return "Disabled";
                    case Trigger.At:
                        return "At";
                    case Trigger.Cron:
                        return "Cron";
                    case Trigger.Queue:
                        return "Workqueue";
                }
                return "";
            }
        }

    }
}
