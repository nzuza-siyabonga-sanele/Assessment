using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Models.Entities;
using Senior_Developer_Assessment.Models.Interfaces;



namespace Senior_Developer_Assessment.Data;

// Class for seeding initial data into the database
public static class SeedData
{
    public static async System.Threading.Tasks.Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new DataDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<DataDbContext>>());

        var authService = serviceProvider.GetRequiredService<IAuthService>();

        // Check if users already exist
        if (await context.Users.AnyAsync())
            return;

        // Create admin user
        await authService.RegisterAsync("admin", "admin@tasktracker.com", "admin123", UserRole.Admin);

        // Create regular user
        await authService.RegisterAsync("user", "user@tasktracker.com", "user123", UserRole.User);

        // Get users
        var admin = await context.Users.FirstAsync(u => u.Username == "admin");
        var user = await context.Users.FirstAsync(u => u.Username == "user");

        // Create sample tasks
        var tasks = new List<Models.Entities.Task>
        {
            new Models.Entities.Task
            {
                Title = "Design Database Schema",
                Description = "Create the initial database schema for the task tracker application",
                Status = Models.Entities.TaskStatus.Completed,
                DueDate = DateTime.UtcNow.AddDays(-5),
                CreatedDate = DateTime.UtcNow.AddDays(-10),
                AssignedUserId = admin.Id // Change this to admin.Id if AssignedUserId is Guid, or use admin.Id.GetHashCode() if you must use int (not recommended)
            },
            new Models.Entities.Task
            {
                Title = "Implement Authentication",
                Description = "Add JWT authentication with refresh tokens",
                Status = Models.Entities.TaskStatus.InProgress,
                DueDate = DateTime.UtcNow.AddDays(3),
                CreatedDate = DateTime.UtcNow.AddDays(-2),
                AssignedUserId = admin.Id
            },
            new Models.Entities.Task
            {
                Title = "Create Task Management UI",
                Description = "Build the frontend interface for managing tasks",
                Status = Models.Entities.TaskStatus.New,
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow.AddDays(-1),
                AssignedUserId = user.Id
            },
            new Models.Entities.Task
            {
                Title = "Write Unit Tests",
                Description = "Create comprehensive unit tests for all services",
                Status = Models.Entities.TaskStatus.New,
                DueDate = DateTime.UtcNow.AddDays(5),
                CreatedDate = DateTime.UtcNow,
                AssignedUserId = user.Id
            }
        };

        await context.Tasks.AddRangeAsync(tasks);
        await context.SaveChangesAsync();
        return;
    }
}