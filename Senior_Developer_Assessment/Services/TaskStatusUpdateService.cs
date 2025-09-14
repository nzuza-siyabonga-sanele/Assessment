

using Senior_Developer_Assessment.Models.Interfaces;

namespace Senior_Developer_Assessment.Services;

// Background service to update task statuses periodically
public class TaskStatusUpdateService : BackgroundService
{
    private readonly ILogger<TaskStatusUpdateService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly int _updateIntervalHours;

    public TaskStatusUpdateService(
        ILogger<TaskStatusUpdateService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _updateIntervalHours = configuration.GetValue<int>("TaskUpdateIntervalHours", 1);
    }

    // Periodically update task statuses
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Task Status Update Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var taskService = scope.ServiceProvider.GetRequiredService<ITaskService>();

                    var updated = await taskService.MarkOverdueTasksAsync();
                    _logger.LogInformation("Task status update completed. {Count} tasks marked as overdue.", updated);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task statuses.");
            }

            await Task.Delay(TimeSpan.FromHours(_updateIntervalHours), stoppingToken);
        }

        _logger.LogInformation("Task Status Update Service is stopping.");
    }
}