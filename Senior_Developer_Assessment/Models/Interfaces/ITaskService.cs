using TaskTracker.API.DTOs;

namespace Senior_Developer_Assessment.Models.Interfaces; 

// Interface using DTOs for security and display purposes
public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetTasksByStatusAsync(Entities.TaskStatus status);
    Task<IEnumerable<TaskDto>> GetTasksByAssigneeAsync(int userId);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
    Task<bool> UpdateTaskAsync(int taskId, UpdateTaskDto dto);
    Task<bool> UpdateTaskStatusAsync(int taskId, Entities.TaskStatus status);
    Task<bool> DeleteTaskAsync(int id);
    Task<bool> MarkOverdueTasksAsync();
}
