using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_SessionIncidents")]
    public class SessionIncident
    {
        public Guid SessionIncidentId { get; set; }
        public Guid SessionId { get; set; }
        public Guid BPSessionId { get; set; }
        public Guid BPResourceId { get; set; }
        public IncidentResolution Resolution { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Closed { get; set; }
    }
}