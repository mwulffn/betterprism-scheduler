using PetaPoco;
using System;

namespace Odk.Scheduler.Database.Models
{
    [TableName("scheduler_Sessions")]
    public class Session
    {
        public Guid SessionId { get; set; }
        public Guid? TaskId { get; set; }
        public SessionState State { get; set; }
        public DateTime DelayStateTransition { get; set; }
        public Guid? BPSessionId { get; set; }
        public Guid? AllocatedResource { get; set; }
        public bool Master { get; set; }
        public bool StopRequested { get; set; }
        public Guid? Launch { get; set; }
        public Guid? Run { get; set; }
        public Guid? Complete { get; set; }
        public Guid? Fail { get; set; }
        public string LaunchParameters { get; set; }
        public string RunParameters { get; set; }
        public string CompleteParameters { get; set; }
        public string FailParameters { get; set; }
        public int LaunchPcd { get; set; }
        public int RunPcd { get; set; }
        public int CompletePcd { get; set; }
        public int FailPcd { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Dispatched { get; set; }
        public DateTime? Closed { get; set; }

        public bool IsInARunnableState()
        {
            if (State == SessionState.Ready || State == SessionState.Completed || State == SessionState.Failed)
                return false;

            return true;
        }

        public Guid ProcessIdForState()
        {
            switch (State)
            {
                case SessionState.Ready:
                    throw new Exception("Unable to launch a ready state session");
                case SessionState.Launching:
                    return Launch.Value;
                case SessionState.Running:
                    return Run.Value;
                case SessionState.Completing:
                    return Complete.Value;
                case SessionState.Completed:
                    throw new Exception("Unable to get a process for a completed session");
                case SessionState.Failing:
                    return Fail.Value;
                case SessionState.Failed:
                    throw new Exception("Unable to get a process for a failed session");
                default:
                    throw new Exception("??");
            }
        }

        public string ParametersForState()
        {
            switch (State)
            {
                case SessionState.Ready:
                    throw new Exception("No parameters exist for a ready state");
                case SessionState.Launching:
                    return LaunchParameters;
                case SessionState.Running:
                    return RunParameters;
                case SessionState.Completing:
                    return CompleteParameters;
                case SessionState.Completed:
                    throw new Exception("No parameters exist for a completed stage");
                case SessionState.Failing:
                    return FailParameters;
                case SessionState.Failed:
                    throw new Exception("No parameters exist for a failed stage");
                default:
                    throw new Exception("??");
            }
        }

        public int PostCompletionDelayForState()
        {
            switch (State)
            {
                case SessionState.Ready:
                    return 0;
                case SessionState.Launching:
                    return LaunchPcd;
                case SessionState.Running:
                    return RunPcd;
                case SessionState.Completing:
                    return CompletePcd;
                case SessionState.Completed:
                    return 0;
                case SessionState.Failing:
                    return FailPcd;
                case SessionState.Failed:
                    throw new Exception("No pcd exist for a failed stage");
                default:
                    throw new Exception("??");
            }
        }

        public void NextState()
        {
            switch (State)
            {
                case SessionState.Ready:
                    if (Launch.HasValue)
                    {
                        State = SessionState.Launching;
                        return;
                    }

                    if (Run.HasValue)
                    {
                        State = SessionState.Running;
                        break;
                    }

                    if (Complete.HasValue)
                    {
                        State = SessionState.Completing;
                        break;
                    }

                    State = SessionState.Completed;
                    break;
                case SessionState.Launching:
                    if (Run.HasValue)
                    {
                        State = SessionState.Running;
                        break;
                    }

                    if (Complete.HasValue)
                    {
                        State = SessionState.Completing;
                        break;
                    }

                    State = SessionState.Completed;
                    break;
                case SessionState.Running:
                    if (Complete.HasValue)
                    {
                        State = SessionState.Completing;
                        break;
                    }

                    State = SessionState.Completed;
                    break;
                case SessionState.Completing:
                    State = SessionState.Completed;
                    break;
                case SessionState.Completed:
                    break;
                case SessionState.Failing:
                    State = SessionState.Failed;
                    break;
                case SessionState.Failed:
                    break;
            }
        }

        public void FailSession()
        {
            if (State == SessionState.Failing)
            {
                State = SessionState.Failed;
                return;
            }

            if (Fail.HasValue)
                State = SessionState.Failing;
            else
                State = SessionState.Failed;
        }
    }
}