using System.ComponentModel.DataAnnotations;

namespace Senior_Developer_Assessment.Models.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Task> Tasks { get; set; } = new List<Task>();

}

public enum UserRole
{
    Admin,
    User
}