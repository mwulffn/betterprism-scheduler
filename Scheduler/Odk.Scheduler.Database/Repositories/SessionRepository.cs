using Odk.Scheduler.Database.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Odk.Scheduler.Database.Repositories
{
    class SessionRepository : Repository<Guid, Session>, ISessionRepository
    {
        public SessionRepository(IDatabase database) : base(database)
        {

        }

        public bool SessionExistsForTask(Task task)
        {
            var sessions = database.Fetch<Session>("WHERE TaskId = @0 AND (State IN (0,1,2,3,5)  OR (State IN (4,6) AND AllocatedResource IS NOT NULL))", task.TaskId);

            return sessions.Count() > 0;
        }

        public IEnumerable<Session> ActiveSessions()
        {
            return database.Fetch<Session>("WHERE State IN (0,1,2,3,5)  OR (State IN (4,6) AND AllocatedResource IS NOT NULL)");
        }

        public Session SessionFromBPSession(Guid bpSessionId)
        {
            return database.SingleOrDefault<Session>("WHERE BPSessionId = @0", bpSessionId);
        }

        public IEnumerable<Session> SessionHistory(int pastDays)
        {
            return database.Fetch<Session>("WHERE Created > @0 ORDER BY Created Desc", DateTime.Today.AddDays(-1 * Math.Abs(pastDays)));
        }

        public SessionIncident GetSessionIncident(Guid incidentId)
        {
            return database.Single<SessionIncident>(incidentId);
        }

        public SessionIncident AddSessionIncident(SessionIncident incident)
        {
            database.Insert(incident);
            return incident;
        }
        public void UpdateSessionIncident(SessionIncident incident)
        {
            database.Save(incident);
        }

        public IEnumerable<SessionIncident> ListPendingIncidents()
        {
            return database.Fetch<SessionIncident>("WHERE Resolution = @0 ORDER BY Created ASC;", IncidentResolution.Unresolved);
        }

        public IEnumerable<SessionIncident> FindSimilarIncidents(Guid incidentId)
        {
            var incident = GetSessionIncident(incidentId);
            var session = this.Single(incident.SessionId);

            var similar = database.Fetch<SessionIncident>("  SELECT * FROM [scheduler_SessionIncidents] LEFT JOIN scheduler_Sessions on scheduler_Sessions.SessionId = scheduler_SessionIncidents.SessionId WHERE TaskId = @0 AND Resolution = 0 AND[scheduler_SessionIncidents].[Closed] IS NULL AND[scheduler_SessionIncidents].Created >= @1 AND[scheduler_SessionIncidents].Created <= @2",
                session.TaskId, incident.Created.AddDays(-1), incident.Created.AddDays(1));

            return similar;
        }
    }
}