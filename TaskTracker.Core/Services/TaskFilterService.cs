using TaskTracker.Core.Models;

namespace TaskTracker.Core.Services;

public static class TaskFilterService
{
    public static IEnumerable<BugReportTask> GetHighSeverityIncompleteBugs(IEnumerable<BaseTask> tasks)
    {
        return tasks
            .OfType<BugReportTask>()
            .Where(b => !b.IsCompleted && b.SeverityMode == SeverityLevel.High)
            .OrderByDescending(b => b.CreatedAt);
    }

    public static int GetTotalEstimatedHoursForIncompleteFeatures(IEnumerable<BaseTask> tasks)
    {
        return tasks
            .OfType<FeatureRequestTask>()
            .Where(f => !f.IsCompleted)
            .Sum(f => f.EstimatedHours);
    }
}
