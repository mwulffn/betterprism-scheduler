using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database
{
    public interface ISessionRepository : IRepository<Guid, Session>
    {
        bool SessionExistsForTask(Task task);
        IEnumerable<Session> ActiveSessions();
        Session SessionFromBPSession(Guid bpSessionId);
        IEnumerable<Session> SessionHistory(int pastDays);
        IEnumerable<SessionIncident> ListPendingIncidents();
        void UpdateSessionIncident(SessionIncident incident);
        SessionIncident GetSessionIncident(Guid incidentId);
        SessionIncident AddSessionIncident(SessionIncident incident);
        IEnumerable<SessionIncident> FindSimilarIncidents(Guid incidentId);
    }
}