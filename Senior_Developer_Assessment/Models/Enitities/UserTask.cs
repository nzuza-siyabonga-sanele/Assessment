namespace Senior_Developer_Assessment.Models.Enitities
{

    public class UserTask
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.New;

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }  // Navigation to User

        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        Delayed,
        Overdue

    }
}