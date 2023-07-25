using System;

namespace Odk.BluePrism
{
    public class BPSession
    {
        public Guid Id { get; set; }
        public int SessionNumber { get; set; }
        public Guid ProcessId { get; set; }
        public string ProcessName { get; set; }
        public BPSessionStatus SessionStatus { get; set; }
        public string SessionStatusName { get { return SessionStatus.ToString(); } }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Resource { get; set; }
        public String User { get; set; }
    }
}