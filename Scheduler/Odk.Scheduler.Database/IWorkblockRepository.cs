using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database
{
    public interface IWorkblockRepository : IRepository<Guid, Workblock>
    {
        IEnumerable<Workblock> All();
        bool IsWorkblockInUse(Workblock workblock);
    }
}