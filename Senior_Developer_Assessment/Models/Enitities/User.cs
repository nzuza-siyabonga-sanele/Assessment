namespace Senior_Developer_Assessment.Models.Enitities;

public class User
{

    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }

    public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>(); // Navigation property: one user -> many tasks


}
