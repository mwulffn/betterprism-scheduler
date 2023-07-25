using Odk.BluePrism;
using Odk.Scheduler.Database.Models;
using System;

namespace Odk.Scheduler.Dto
{
    public class SessionMonitorItem
    {
        public SessionIncident Incident { get; }
        public Guid TaskId { get; set; }
        public string Task { get; set; }
        public string Resource { get; set; }
        public string ProcessName { get; set; }
        public BPLogEntry BPLogEntry { get; set; }
        public SessionMonitorItem(SessionIncident incident)
        {
            Incident = incident;
        }
    }
}