using shukuma.domain.Models;

namespace shukuma.domain.Interfaces
{
    public interface IUserService
    {
        // Authentication
        Task<ResponseVm> SignUp(SignupModel user);
        Task<ResponseVm> SignIn(SigninModel user);
        Task<ResponseVm> UpdatedUserInfo(UserModel user);

        // User Management
        Task<List<UserInfo>> GetUsers();
        Task<UserInfo?> GetUserById(string userId);
        Task<UserInfo?> GetUserByEmail(string email);
        Task<ResponseVm> UpdateUserProfile(UserInfo user);

        // Workout Sessions
        Task<ResponseVm> SaveWorkoutSession(WorkoutSession session);
        Task<List<WorkoutSession>> GetUserWorkoutHistory(string userId);
        Task<UserStats?> GetUserStats(string userId);

        // Leaderboard
        Task<List<LeaderboardEntry>> GetLeaderboard(int topN = 10);
        Task<List<LeaderboardEntry>> GetLeaderboardByBestTime(int topN = 10);
        Task<List<LeaderboardEntry>> GetLeaderboardByMostWorkouts(int topN = 10);
    }
}