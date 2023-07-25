using Odk.Scheduler.Database.Models;
using System;
using System.Collections.Generic;

namespace Odk.Scheduler.Database
{
    public interface ITaskRepository : IRepository<Guid,Task>
    {
        Session SessionFromTask(Task task, bool masterTask = true);
        IEnumerable<Task> EnabledTasks();
        IEnumerable<Task> AllTasks();
        IEnumerable<AtDate> AllAtDates(Guid taskId);
        IEnumerable<AtDate> ActiveAtDates(Guid taskId);
        AtDate AddAtDate(AtDate date);
        void DeleteAtDate(int atDateId);
        void RegisterTrigger(AtDate date);
    }
}