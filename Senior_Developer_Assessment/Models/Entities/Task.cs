using System.ComponentModel.DataAnnotations;

namespace Senior_Developer_Assessment.Models.Entities;

public class Task : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public TaskStatus Status { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public int AssignedUserId { get; set; }

}

public enum TaskStatus
{
    New = 0,
    InProgress = 1,
    Completed = 2,
    Overdue = 3
}