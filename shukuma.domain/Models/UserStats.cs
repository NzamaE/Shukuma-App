namespace shukuma.domain.Models;

public class UserStats
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TotalWorkouts { get; set; } = 0;
    public int TotalCardsCompleted { get; set; } = 0;
    public string BestTime { get; set; } = string.Empty;
    public string AverageTime { get; set; } = string.Empty;
    public DateTime? LastWorkoutDate { get; set; }
    public DateTime MemberSince { get; set; }
    public List<WorkoutSession> RecentWorkouts { get; set; } = new();
    public bool HasCompleted52Cards { get; set; } = false;
    public int CurrentStreak { get; set; } = 0; // Days in a row
}