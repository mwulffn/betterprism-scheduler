using System;

namespace Odk.Scheduler.Database.Models
{
    /// <summary>
    /// Class for grouping screenshots. It has no database table equivalent.
    /// </summary>
    public class ScreenshotSessionGroup
    {
        public Guid SessionId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime Created { get; set; }
    }
}