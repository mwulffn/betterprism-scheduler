using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_AtDates")]
    [PrimaryKey("Id",AutoIncrement = true)]
    public class AtDate
    {
        public int Id { get; set; }
        [PetaPoco.Column("AtDate")]
        public DateTime At { get; set; }
        public Guid TaskId { get; set; }
        public bool HasBeenTriggered { get; set; }
    }
}