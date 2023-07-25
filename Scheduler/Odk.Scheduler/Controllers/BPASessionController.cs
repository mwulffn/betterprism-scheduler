
using Odk.BluePrism;
using Odk.Scheduler.Database;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Controllers
{
    public class BPASessionController : BaseController<Guid, object>
    {
        public BPASessionController(IBluePrism bluePrism,ISessionRepository sessionRepository) : base(bluePrism)
        {

        }

        public override IEnumerable<object> Get()
        {
            return bluePrism.LatestSessions(50);
        }
    }
}