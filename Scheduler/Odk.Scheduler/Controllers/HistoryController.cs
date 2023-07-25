using Odk.BluePrism;
using Odk.Scheduler.Database;
using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Controllers
{
    public class HistoryController : BaseController<Guid, Session>
    {
        private readonly ISessionRepository sessionRepository;

        public HistoryController(ISessionRepository sessionRepository, IBluePrism bluePrism) : base(bluePrism)
        {
            this.sessionRepository = sessionRepository;
        }

        public override IEnumerable<Session> Get()
        {
            return sessionRepository.SessionHistory(14);
        }
    }
}