namespace Senior_Developer_Assessment.DTOs;

// DTO for transferring task data with necessary fields
public class TaskDto
{

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required string Status { get; set; }
    public DateTime DueDate { get; set; }

    // Include assigned user info
    public int AssignedUserId { get; set; }
}

// DTO for creating a new task with necessary fields
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Models.Entities.TaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }

    // Include assignedUserId
    public int AssignedUserId { get; set; }

}

// DTO for updating task status
public class UpdateStatusRequest
{
    public Models.Entities.TaskStatus Status { get; set; }
}

// DTO for updating task details
public class UpdateTaskDto
{
 
    public required string Title { get; set; } = null!;
    public string? Description { get; set; }

    public required Models.Entities.TaskStatus Status { get; set; }

    public required DateTime DueDate { get; set; }

    public int AssignedUserId { get; set; }
}
