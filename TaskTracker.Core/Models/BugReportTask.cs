namespace TaskTracker.Core.Models;

public class BugReportTask : BaseTask
{
    public required SeverityLevel SeverityMode { get; set; }
}
