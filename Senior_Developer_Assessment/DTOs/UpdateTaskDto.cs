using System.ComponentModel.DataAnnotations;

namespace Senior_Developer_Assessment.DTOs;

// DTO for updating task details
public class UpdateTaskDto
{
    [Required]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    [Required]
    public Models.Entities.TaskStatus Status { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    public int AssignedUserId { get; set; }
}
