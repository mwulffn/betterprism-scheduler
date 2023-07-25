using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database
{
    public interface IScreenshotRepository : IRepository<Guid,Screenshot>
    {
        IEnumerable<ScreenshotSessionGroup> SessionGroups(string filter);
        IEnumerable<Screenshot> ScreenshotsBySession(Guid sessionId);
    }
}