namespace Odk.Scheduler.Database.Models
{
    public enum SessionState
    {
        Ready = 0,
        Launching = 1,
        Running = 2,
        Completing = 3,
        Completed =4,
        Failing = 5,
        Failed = 6
    }
}