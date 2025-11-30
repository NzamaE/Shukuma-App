
namespace shukuma.domain.Models;

public class UserInfo
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty; // ADDED: Missing field
    public string ContactNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public bool HasCompletedChallenge { get; set; } = false;
    public string CompletedBy { get; set; } = string.Empty;
    public string TimeCompleted { get; set; } = string.Empty;
    public int LastCardNumber { get; set; } = default;
    public string CardsCompleted { get; set; } = string.Empty;
    public string? Review { get; set; }
}