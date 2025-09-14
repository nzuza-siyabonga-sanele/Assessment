

using Senior_Developer_Assessment.DTOs;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;

namespace Senior_Developer_Assessment.Services;

// Service class implementing ITaskService interface for task management
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    private readonly IUserRepository _userRepository;


    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
    }


    // Get all tasks
    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
      
        return tasks.Select(MapToDto);
    }

    // Get task by id
    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task == null ? null : MapToDto(task);
    }

    // Get tasks by status
    public async Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(Models.Entities.TaskStatus status)
    {
        var tasks = await _taskRepository.GetTasksByStatusAsync(status);
        return tasks.Select(MapToDto);
    }

    // Get tasks by assignee
    public async Task<IEnumerable<TaskDto>> GetTasksByAssigneeAsync(int userId)
    {
        var tasks = await _taskRepository.GetTasksByAssigneeAsync(userId);
        return tasks.Select(MapToDto);
    }

    // Get tasks due before a date
    public async Task<IEnumerable<TaskDto>> GetTasksDueBeforeAsync(DateTime date)
    {
        var tasks = await _taskRepository.GetTasksDueBeforeAsync(date);
        return tasks.Select(MapToDto);
    }

    // Get overdue tasks
    public async Task<IEnumerable<TaskDto>> GetOverdueTasksAsync()
    {
        var tasks = await _taskRepository.GetOverdueTasksAsync();
        return tasks.Select(MapToDto);
    }

    // Create a new task
    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
    {
        User? user = null;

        // If an AssignedUserId is provided, fetch the user
        if (dto.AssignedUserId > 0)
        {
            user = await _userRepository.GetByIdAsync(dto.AssignedUserId);
            if (user == null)
                throw new ArgumentException("AssignedUserId does not correspond to an existing user.");
        }

        var task = new Models.Entities.Task
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            AssignedUserId = dto.AssignedUserId
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveAsync();

        // Map the task to DTO including the username
        return MapToDto(task);
    }

    // Update an existing task
    public async Task<bool> UpdateTaskAsync(int taskId, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task == null) return false;

        if (dto.AssignedUserId > 0)
        {
            var user = await _userRepository.GetByIdAsync(dto.AssignedUserId);
            if (user == null)
                throw new ArgumentException("AssignedUserId does not correspond to an existing user.");
        }

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.DueDate = dto.DueDate;
        task.AssignedUserId = dto.AssignedUserId;

        _taskRepository.Update(task);
        return await _taskRepository.SaveAsync();
    }

    // Delete a task
    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null) return false;

        _taskRepository.Delete(task);
        return await _taskRepository.SaveAsync();
    }

    // Update task status
    public async Task<bool> UpdateTaskStatusAsync(int taskId, Models.Entities.TaskStatus status)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null) return false;

        task.Status = status;
        _taskRepository.Update(task);
        return await _taskRepository.SaveAsync();
    }

    // Mark overdue tasks
    public async Task<bool> MarkOverdueTasksAsync()
    {
        // Get all tasks that are overdue
        var overdueTasks = await _taskRepository.GetOverdueTasksAsync();

        foreach (var task in overdueTasks)
        {
            task.Status = Models.Entities.TaskStatus.Overdue;
            _taskRepository.Update(task);
        }

        return await _taskRepository.SaveAsync();

    }

    // Helper method to map Task entity to TaskDto
    private static TaskDto MapToDto(Models.Entities.Task task) => new TaskDto
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description ?? string.Empty,
        Status = task.Status.ToString(),
        DueDate = task.DueDate,
        AssignedUserId = task.AssignedUserId
    };
}