using TaskTracker.Core.Models;

namespace TaskTracker.Core.Interfaces;

public interface ITaskRepository
{
    IEnumerable<BaseTask> GetAll();
    void Add(BaseTask task);
    BaseTask? GetById(Guid id);
}
