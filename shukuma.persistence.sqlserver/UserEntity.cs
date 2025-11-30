using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shukuma.persistence.sqlserver;

[Table("User")]
public class UserEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreatedBy { get; set; } = DateTime.Now;
    public bool HasCompletedChallenge { get; set; } = false;
    public DateTime? CompletedBy { get; set; }
    public string TimeCompleted { get; set; } = string.Empty;
    public int LastCardNumber { get; set; } = default;
    public string CardsCompleted { get; set; } = string.Empty;
    public string? Review { get; set; }
}
