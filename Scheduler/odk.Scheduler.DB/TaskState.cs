namespace odk.Scheduler.DB
{
    public enum TaskState
    {
        Ready = 0,
        Launch = 1,
        Run = 2,
        Complete = 3,
        Fail = 4,
        Completed = 5,
        Failed = 6
    }

    public static class EnumExtensions
    {
        public static string HumanReadable(this TaskState state)
        {
            switch (state)
            {
                case TaskState.Ready:
                    return "Ready";
                case TaskState.Launch:
                    return "Launch";
                case TaskState.Run:
                    return "Run";
                case TaskState.Complete:
                    return "Complete";
                case TaskState.Fail:
                    return "Fail";
                case TaskState.Completed:
                    return "Completed";
                case TaskState.Failed:
                    return "Failed";
            }
            return "";
        }

    }
}