

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Senior_Developer_Assessment.DTOs;
using Senior_Developer_Assessment.Models.Interfaces;

namespace Senior_Developer_Assessment.Controllers;

[ApiController]
[Route("api/[controller]")]

public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    private readonly string _message;


    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
        _message = "successfully";

    }

    // Get all tasks
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        try
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(new { message = _message, Info = tasks });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tasks");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while retrieving tasks" });
        }
    }

    // Get task by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        try
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
                return NotFound();
        
            return Ok(new { message = _message, Info = task });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task with ID {TaskId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while retrieving the task" });
        }
    }

    // Update an existing task
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskDto dto)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _taskService.UpdateTaskAsync(id, dto);

            if (!success)
                return NotFound();

            return Ok(new { message = _message});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task with ID {TaskId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while updating the task" });
        }
    }

    // Update only the status of a task 
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] UpdateStatusRequest dto)
    {
        try
        {
            var success = await _taskService.UpdateTaskStatusAsync(id, dto.Status);

            if (!success)
                return NotFound();

            return Ok(new { message = _message});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating status for task with ID {TaskId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while updating the task status" });
        }
    }

    // Delete a task
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            var success = await _taskService.DeleteTaskAsync(id);

            if (!success)
                return NotFound();

            return Ok(new { message = _message});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task with ID {TaskId}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while deleting the task" });
        }
    }

    // Create a new task
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTask = await _taskService.CreateTaskAsync(dto);            
            return Ok(new {message = _message, Info = CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask)});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "An error occurred while creating the task" });
        }
    }
   
}
