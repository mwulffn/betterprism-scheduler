using Odk.Scheduler.Database.Models;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database.Repositories
{
    class ScreenshotRepository : Repository<Guid, Screenshot>, IScreenshotRepository
    {
        public ScreenshotRepository(IDatabase database) : base(database)
        {

        }

        public IEnumerable<ScreenshotSessionGroup> SessionGroups(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return database.Fetch<ScreenshotSessionGroup>("SELECT ss.SessionId,task.[Name],ss.Created,count(scheduler_screenshots.ScreenshotId) as [Count] FROM [scheduler_Screenshots] JOIN scheduler_Sessions as ss on ss.SessionId = scheduler_Screenshots.SessionId JOIN scheduler_Tasks as task on task.TaskId = ss.TaskId group by ss.SessionId,task.[name],ss.Created order by ss.Created desc;");

            filter = "%" + filter + "%";

            return database.Fetch<ScreenshotSessionGroup>("SELECT ss.SessionId,task.[Name],ss.Created,count(scheduler_screenshots.ScreenshotId) as [Count] FROM [scheduler_Screenshots] JOIN scheduler_Sessions as ss on ss.SessionId = scheduler_Screenshots.SessionId JOIN scheduler_Tasks as task on task.TaskId = ss.TaskId where task.[Name] LIKE(@0) group by ss.SessionId,task.[name],ss.Created order by ss.Created desc;", filter);
        }

        public IEnumerable<Screenshot> ScreenshotsBySession(Guid sessionId)
        {
            return database.Fetch<Screenshot>("WHERE SessionId=@0 ORDER BY Created DESC;", sessionId);
        }
    }
}