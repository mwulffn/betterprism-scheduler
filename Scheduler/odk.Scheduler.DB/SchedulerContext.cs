using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace odk.Scheduler.DB
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext() : base("SchedulerContext")
        {
        }

        public List<Session> GetActiveSessions()
        {
            return Sessions.Where(a => a.Active && a.TaskState != TaskState.Ready && a.DelayUntil <= DateTime.Now).ToList();
        }

        public List<Session> GetReadySessions()
        {
            return Sessions.Where(a => a.Active && a.TaskState == TaskState.Ready && a.DelayUntil <= DateTime.Now).ToList();
        }

        public ActivityLog AddActivityLog(string message)
        {
            var log = new ActivityLog()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Message = message
            };

            this.ActivityLogs.Add(log);

            SaveChanges();

            return log;
        }


        public override int SaveChanges()
        {
            DateTime saveTime = DateTime.Now;

            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == (EntityState)System.Data.Entity.EntityState.Added))
            {


                //
                if (entry.Entity.GetType().GetProperty("Created") != null)
                    entry.Property("Created").CurrentValue = saveTime;

                if (entry.Entity.GetType().GetProperty("Updated") != null)
                    entry.Property("Updated").CurrentValue = saveTime;

            }

            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == (EntityState)System.Data.Entity.EntityState.Modified))
            {
                //

                if (entry.Entity.GetType().GetProperty("Updated") != null)
                    entry.Property("Updated").CurrentValue = saveTime;
            }

            return base.SaveChanges();
        }


        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskWorkblock> TaskWorkblocks { get; set; }
        public virtual DbSet<Workblock> Workblocks { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<AtDate> AtDates { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
    }
}
