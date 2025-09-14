using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Data;
using Senior_Developer_Assessment.Models.Interfaces;


namespace Senior_Developer_Assessment.Models.Repositories;

/// Repository for task-specific data operations
public class TaskRepository : Repository<Models.Entities.Task>, ITaskRepository
{
    public TaskRepository(DataDbContext context) : base(context)
    {
    }

    // Get tasks by status with filtering
    public async Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(Entities.TaskStatus status) =>
        await _context.Tasks.Where(t => t.Status == status)
            .ToListAsync();

    // Get tasks assigned to a specific user with filtering
    public async Task<IEnumerable<Entities.Task>> GetTasksByAssigneeAsync(int userId) =>
        await _context.Tasks.Where(t => t.AssignedUserId == userId)
            .ToListAsync();

    // Get overdue tasks with filtering
    public async Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync() =>
        await _context.Tasks
            .Where(t => t.DueDate < DateTime.UtcNow && t.Status != Entities.TaskStatus.Completed)
            .ToListAsync();

    // Get tasks due before a specific date with filtering
    public async Task<IEnumerable<Entities.Task>> GetTasksDueBeforeAsync(DateTime date) =>
        await _context.Tasks.Where(t => t.DueDate < date)
        .ToListAsync();
}