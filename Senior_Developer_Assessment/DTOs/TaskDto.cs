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