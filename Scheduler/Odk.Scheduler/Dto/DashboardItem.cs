using Odk.Scheduler.Database.Models;
using System;

namespace Odk.Scheduler.Dto
{
    public class DashboardItem
    {
        public Guid Id { get; set; }
        public Guid? TaskId { get; set; }
        public string SessionState { get; set; }        
        public string TaskName { get; set; }
        public Guid ProcessId { get; set; }
        public string ProcessName { get; set; }
        public DateTime DelayUntil { get; set; }
        public Guid BPSessionId { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; }
        public bool Master { get; set; }
        public bool StopRequested { get; set; }
        public DateTime? Dispatched { get; set; }

        public DashboardItem()
        {

        }

        public DashboardItem(Session session)
        {
            Id = session.SessionId;
            TaskId = session.TaskId;
            Dispatched = session.Dispatched;
            SessionState = session.State.ToString();

            // Suppress irrelevant exceptions
            try
            {
                ProcessId = session.ProcessIdForState();
            }
            catch (Exception)
            {

            }

            DelayUntil = session.DelayStateTransition;
            BPSessionId = session.BPSessionId.HasValue ? session.BPSessionId.Value : Guid.Empty;
            ResourceId = session.AllocatedResource.HasValue ? session.AllocatedResource.Value : Guid.Empty;
            Master = session.Master;
            StopRequested = session.StopRequested;
        }
    }
}