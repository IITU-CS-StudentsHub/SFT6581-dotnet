using Microsoft.AspNetCore.Mvc;
using TaskTracker.Core.Interfaces;
using TaskTracker.Core.Models;

namespace TaskTracker.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskRepository taskRepository, ILogger<TasksController> logger) : ControllerBase
{
    private readonly ITaskRepository _taskRepository = taskRepository;
    private readonly ILogger<TasksController> _logger = logger;

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var tasks = _taskRepository.GetAll();
        return Ok(tasks);
    }

    [HttpPost("bug")]
    public IActionResult CreateBugReport([FromBody] CreateBugReportDto dto)
    {
        var bug = new BugReportTask
        {
            Title = dto.Title,
            SeverityMode = dto.SeverityMode
        };

        _taskRepository.Add(bug);
        return CreatedAtAction(nameof(GetAllTasks), new { id = bug.Id }, bug);
    }

    [HttpPut("{id:guid}/complete")]
    public IActionResult CompleteTask(Guid id)
    {
        var task = _taskRepository.GetById(id);
        
        if (task == null)
            return NotFound(new { Message = "Task not found" });

        if (task.IsCompleted)
            return BadRequest(new { Message = "Task is already completed" });

        // Subscribe local handler for logging
        void OnTaskCompletedHandler(BaseTask completedTask)
        {
            _logger.LogInformation("Task '{Title}' ({Id}) was completed at {Time}", 
                completedTask.Title, completedTask.Id, DateTime.UtcNow);
        }

        task.OnTaskCompleted += OnTaskCompletedHandler;

        try
        {
            task.CompleteTask();
        }
        finally
        {
            // Unsubscribe to avoid memory leaks if the task stays in memory
            task.OnTaskCompleted -= OnTaskCompletedHandler;
        }

        return Ok(new { Message = "Task completed successfully", Task = task });
    }
}

public record CreateBugReportDto(string Title, SeverityLevel SeverityMode);
