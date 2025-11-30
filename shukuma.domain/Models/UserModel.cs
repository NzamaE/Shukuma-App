namespace shukuma.domain.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty; // ADD THIS
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty; // ADD THIS
        public string EmailAddress { get; set; } = string.Empty;
        public string CompletedBy { get; set; } = string.Empty;
        public string TimeCompleted { get; set; } = string.Empty;
        public string CardsCompleted { get; set; } = string.Empty;
        public string? Review { get; set; }

        public string FullName => $"{FirstName} {LastName}"; // ADD THIS
    }
}