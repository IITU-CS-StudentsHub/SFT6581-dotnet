namespace TaskTracker.Core.Models;

public delegate void TaskCompletedHandler(BaseTask task);

public abstract class BaseTask
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public bool IsCompleted { get; private set; }

    public event TaskCompletedHandler? OnTaskCompleted;

    public void CompleteTask()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        OnTaskCompleted?.Invoke(this);
    }
}
