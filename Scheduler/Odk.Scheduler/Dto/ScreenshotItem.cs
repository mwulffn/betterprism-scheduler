using System;

namespace Odk.Scheduler.Dto
{
    public class ScreenshotItem
    {
        public string Screenshot { get; set; }
        public string Mimetype { get; set; }
        public Guid BPSessionId { get; set; }
        public string ItemKey { get; set; }
        public byte[] ScreenshotBin
        {
            get
            {
                return Convert.FromBase64String(Screenshot);
            }
        }
    }
}