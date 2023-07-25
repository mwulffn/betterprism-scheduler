using System;
using System.Linq;

namespace odk.Scheduler.DB
{
    public class Session
    {
        public Guid Id { get; set; }
        public TaskState TaskState { get; set; }
        public Guid BPSessionId { get; set; }
        public DateTime DelayUntil { get; set; }
        public bool Master { get; set; }

        public Guid? AllocatedResource { get; set; }

        public virtual Task Task { get; set; }

        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }


        public bool Active { get; set; }
        public bool StopRequested { get; set; }

        // Returns true if there is more work to be done.
        public bool NextStage()
        {

            switch (TaskState)
            {
                case TaskState.Fail:
                    TaskState = TaskState.Failed;
                    Active = false;
                    BPSessionId = Guid.Empty;
                    return false;
                case TaskState.Complete:
                    TaskState = TaskState.Completed;
                    Active = false;
                    BPSessionId = Guid.Empty;
                    return false;
                case TaskState.Run:
                    {
                        BPSessionId = Guid.Empty;
                        if (Task.TaskWorkblocks.Any(a => a.Workblock.Intention == TaskState.Complete))
                        {
                            DelayUntil = DateTime.Now.AddSeconds(CurrentWorkblock.PostCompletionDelay);
                            TaskState = TaskState.Complete;
                            Active = true;
                            return true;
                        }
                        else
                        {
                            TaskState = TaskState.Completed;
                            Active = false;
                            return false;
                        }
                    }

                case TaskState.Launch:
                    {
                        BPSessionId = Guid.Empty;

                        if (Task.TaskWorkblocks.Any(a => a.Workblock.Intention == TaskState.Run))
                        {
                            DelayUntil = DateTime.Now.AddSeconds(CurrentWorkblock.PostCompletionDelay);
                            TaskState = TaskState.Run;
                            return true;
                        }
                        else
                        {
                            TaskState = TaskState.Completed;
                            Active = false;
                            return false;
                        }
                    }

                case TaskState.Ready:
                    {
                        BPSessionId = Guid.Empty;

                        if (Task.TaskWorkblocks.Any(a => a.Workblock.Intention == TaskState.Launch))
                        {
                            TaskState = TaskState.Launch;
                            Active = true;
                            return true;
                        }

                        if (Task.TaskWorkblocks.Any(a => a.Workblock.Intention == TaskState.Run))
                        {
                            TaskState = TaskState.Run;
                            Active = true;
                            return true;
                        }

                        break;
                    }
            }

            return false;

        }

        public bool FailSession()
        {
            if (Task.TaskWorkblocks.Any(a => a.Workblock.Intention == TaskState.Fail) && this.TaskState != TaskState.Fail)
            {
                TaskState = TaskState.Fail;
                Active = true;
                return true;
            }
            else
            {
                TaskState = TaskState.Failed;
                Active = false;
                return false;
            }

        }


        public Workblock CurrentWorkblock
        {
            get
            {
                return Task.TaskWorkblocks.SingleOrDefault(a => a.Workblock.Intention == TaskState)?.Workblock;
            }
        }

        public TaskWorkblock CurrentTaskWorkblock
        {
            get
            {
                return Task.TaskWorkblocks.SingleOrDefault(a => a.Workblock.Intention == TaskState);
            }
        }



    }
}
