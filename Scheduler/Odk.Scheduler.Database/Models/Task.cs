using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_Tasks")]
    public class Task
    {
        public Guid TaskId { get; set; }
        public string Name { get; set; }
        public Trigger Trigger { get; set; }
        public bool Enabled { get; set; }
        public string Cron { get; set; }
        public Guid? Workqueue { get; set; }
        public int ScaleLimit { get; set; }
        public int ScaleThreshold { get; set; }
        public Guid? Launch { get; set; }
        public Guid? Run { get; set; }
        public Guid? Complete { get; set; }
        public Guid? Fail { get; set; }
        public DateTime? LastTriggered { get; set; }
        public bool Deleted { get; set; }
    }
}