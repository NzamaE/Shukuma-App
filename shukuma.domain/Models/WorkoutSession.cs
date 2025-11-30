namespace shukuma.domain.Models;

public class WorkoutSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int CardsCompleted { get; set; } = 0;
    public string TimeElapsed { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; } = false;
    public int[] CardSequence { get; set; } = Array.Empty<int>(); // Track which cards were used
}