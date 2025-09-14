
namespace Senior_Developer_Assessment.Models.Interfaces;    

// Interface using task entities class for full information
public interface ITaskRepository : IRepository<Entities.Task>
{
    Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(Entities.TaskStatus status);
    Task<IEnumerable<Entities.Task>> GetTasksByAssigneeAsync(int userId);
    Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
    Task<IEnumerable<Entities.Task>> GetTasksDueBeforeAsync(DateTime date);
}