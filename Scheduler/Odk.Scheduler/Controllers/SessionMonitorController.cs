using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using Odk.Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Odk.Scheduler.Controllers
{
    [RoutePrefix("api/sessionmonitor")]
    public class SessionMonitorController : BaseController<Guid, SessionMonitorItem>
    {
        private readonly ISessionRepository sessionRepository;
        private readonly ITaskRepository taskRepository;

        public SessionMonitorController(IBluePrism bluePrism, ISessionRepository sessionRepository, ITaskRepository taskRepository) : base(bluePrism)
        {
            this.sessionRepository = sessionRepository;
            this.taskRepository = taskRepository;
        }

        public override IEnumerable<SessionMonitorItem> Get()
        {
            var items = sessionRepository.ListPendingIncidents();

            return items.Select(a => Inflate(a));
        }

        public override SessionMonitorItem Post([FromBody] SessionMonitorItem obj)
        {
            var incident = sessionRepository.GetSessionIncident(obj.Incident.SessionIncidentId);
            incident.Resolution = obj.Incident.Resolution;

            if (incident.Resolution != IncidentResolution.Unresolved)
                incident.Closed = DateTime.Now;

            sessionRepository.UpdateSessionIncident(incident);

            return obj;
        }

        [HttpPost]
        [Route("resolve-similar")]
        public void ResolveSimilar([FromBody] SessionMonitorItem obj)
        {
            var incident = sessionRepository.GetSessionIncident(obj.Incident.SessionIncidentId);
            var similar = sessionRepository.FindSimilarIncidents(incident.SessionIncidentId);

            foreach (var item in similar)
            {
                item.Resolution = IncidentResolution.Dismissed;
                item.Closed = DateTime.Now;
                sessionRepository.UpdateSessionIncident(item);
            }
        }

        private SessionMonitorItem Inflate(SessionIncident incident)
        {
            var result = new SessionMonitorItem(incident);
            var session = sessionRepository.Single(incident.SessionId);
            var task = taskRepository.SingleOrDefault(session.TaskId.Value);
            var bpsession = bluePrism.GetSession(incident.BPSessionId);
            result.ProcessName = bpsession.ProcessName;
            result.Resource = bluePrism.GetResource(incident.BPResourceId).Name;

            if (task != null)
            {
                result.TaskId = task.TaskId;
                result.Task = task.Name;
            }

            result.BPLogEntry = bluePrism.GetLastLogEntry(incident.BPSessionId);

            return result;
        }
    }
}