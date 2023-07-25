using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_Screenshots")]
    public class Screenshot
    {
        public Guid ScreenshotId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid BPSessionId { get; set; }
        public string Mimetype { get; set; }
        public string ItemKey { get; set; }
        [Column("Screenshot")]
        public byte[] ScreenshotData { get; set; }
        public DateTime Created { get; set; }
    }
}