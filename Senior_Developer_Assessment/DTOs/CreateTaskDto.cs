

// DTO for creating a new task with necessary fields
public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public DateTime DueDate { get; set; }

    // Include assignedUserId
    public int AssignedUserId { get; set; }

}
