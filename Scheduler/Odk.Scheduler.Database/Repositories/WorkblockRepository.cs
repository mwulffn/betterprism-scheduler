using Odk.Scheduler.Database.Models;
using PetaPoco;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database.Repositories
{
    class WorkblockRepository : Repository<Guid, Workblock>, IWorkblockRepository
    {
        public WorkblockRepository(IDatabase database) : base(database)
        {

        }

        public IEnumerable<Workblock> All()
        {
            return database.Fetch<Workblock>("WHERE Deleted = 0 ORDER BY Intention ASC, Name ASC");
        }

        public bool IsWorkblockInUse(Workblock workblock)
        {
            return database.ExecuteScalar<int>("SELECT COUNT(*) FROM [scheduler_Tasks] WHERE Deleted = 0 AND (Launch = @0 OR Run = @0 OR Complete = @0 OR Fail = @0);", workblock.WorkblockId) > 0;
        }
    }
}