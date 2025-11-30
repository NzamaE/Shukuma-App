namespace shukuma.domain.Models;

public class LeaderboardEntry
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int TotalWorkouts { get; set; } = 0;
    public int TotalCardsCompleted { get; set; } = 0;
    public string BestTime { get; set; } = string.Empty;
    public int BestTimeInMilliseconds { get; set; } = int.MaxValue;
    public DateTime? LastWorkoutDate { get; set; }
    public bool HasCompleted52Cards { get; set; } = false;
}