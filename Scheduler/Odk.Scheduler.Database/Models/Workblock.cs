using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_Workblocks")]
   
    public class Workblock
    {
        public Guid WorkblockId { get; set; }
        public Guid ProcessId { get; set; }
        public string Name { get; set; }
        public int PostCompletionDelay { get; set; }
        public Intention Intention { get; set; }
        public string Parameters { get; set; }

        [Ignore]
        public string ProcessName { get; set; }

        public bool Deleted { get; set; }
    }
}