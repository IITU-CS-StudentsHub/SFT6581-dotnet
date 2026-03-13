using System.Collections.Concurrent;
using TaskTracker.Core.Interfaces;
using TaskTracker.Core.Models;

namespace TaskTracker.Web.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly ConcurrentBag<BaseTask> _tasks = new();

    public IEnumerable<BaseTask> GetAll() => _tasks;

    public void Add(BaseTask task) => _tasks.Add(task);

    public BaseTask? GetById(Guid id) => _tasks.FirstOrDefault(t => t.Id == id);
}
