using AutoMapper;
using Google.Cloud.Firestore;
using shukuma.domain.Interfaces;
using shukuma.domain.Models;

namespace shukuma.persistence.firebase;

public class UserServiceFB : IUserService
{
    readonly FirestoreDb _database;
    readonly IMapper _mapper;
    readonly FirestoreAppOptions _appOptions;
    readonly IHashService _hashService;

    public UserServiceFB(FirestoreDb database, FirestoreAppOptions appOptions, IHashService hashService)
    {
        _database = database;
        _appOptions = appOptions;
        _hashService = hashService;

        var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = new Mapper(mappingConfig);
    }

    public async Task<List<UserInfo>> GetUsers()
    {
        try
        {
            var query = _database.Collection(_appOptions.Collection.User);
            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Any())
            {
                var users = new List<UserInfo>();
                foreach (var doc in snapshot.Documents)
                {
                    var entity = doc.ConvertTo<UserEntity>();
                    var user = _mapper.Map<UserInfo>(entity);
                    users.Add(user);
                }
                return users;
            }
            return new List<UserInfo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting users: {ex.Message}");
            return new List<UserInfo>();
        }
    }

    public async Task<UserInfo?> GetUserById(string userId)
    {
        try
        {
            var docRef = _database.Collection(_appOptions.Collection.User).Document(userId);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                var entity = snapshot.ConvertTo<UserEntity>();
                return _mapper.Map<UserInfo>(entity);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user by ID: {ex.Message}");
            return null;
        }
    }

    public async Task<UserInfo?> GetUserByEmail(string email)
    {
        try
        {
            var users = _database.Collection(_appOptions.Collection.User);
            var query = users.WhereEqualTo("EmailAddress", email.Trim().ToLower());
            var querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Any())
            {
                var entity = querySnapshot.First().ConvertTo<UserEntity>();
                return _mapper.Map<UserInfo>(entity);
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user by email: {ex.Message}");
            return null;
        }
    }

    public async Task<ResponseVm> SignUp(SignupModel user)
    {
        try
        {
            var normalizedEmail = user.EmailAddress.Trim().ToLower();
            var users = _database.Collection(_appOptions.Collection.User);
            var query = users.WhereEqualTo("EmailAddress", normalizedEmail);
            var querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Any())
            {
                return new ResponseVm { IsSuccess = false, ErrorMessage = $"Account with {user.EmailAddress} already exists." };
            }

            var userEntity = _mapper.Map<UserEntity>(user);
            userEntity.Id = Guid.NewGuid().ToString();
            userEntity.EmailAddress = normalizedEmail;
            userEntity.Password = _hashService.GetHash(user.Password);
            userEntity.CreatedBy = DateTime.UtcNow.ToString("O");
            userEntity.CardsCompleted = "0";
            userEntity.TimeCompleted = "00:00:00";

            var docRef = _database.Collection(_appOptions.Collection.User).Document(userEntity.Id);
            await docRef.SetAsync(userEntity);

            return new ResponseVm { IsSuccess = true, User = _mapper.Map<UserModel>(userEntity) };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sign up error: {ex.Message}");
            return new ResponseVm { IsSuccess = false, ErrorMessage = "Registration failed." };
        }
    }

    public async Task<ResponseVm> SignIn(SigninModel user)
    {
        try
        {
            var users = _database.Collection(_appOptions.Collection.User);
            var query = users.WhereEqualTo("EmailAddress", user.EmailAddress.Trim().ToLower());
            var querySnapshot = await query.GetSnapshotAsync();

            if (!querySnapshot.Any())
            {
                return new ResponseVm { IsSuccess = false, ErrorMessage = $"No account found with {user.EmailAddress}." };
            }

            var userEntity = querySnapshot.First().ConvertTo<UserEntity>();
            if (userEntity.Password != _hashService.GetHash(user.Password))
            {
                return new ResponseVm { IsSuccess = false, ErrorMessage = "Incorrect password." };
            }

            return new ResponseVm { IsSuccess = true, User = _mapper.Map<UserModel>(userEntity) };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Sign in error: {ex.Message}");
            return new ResponseVm { IsSuccess = false, ErrorMessage = "Sign in failed." };
        }
    }

    public async Task<ResponseVm> UpdateUserProfile(UserInfo user)
    {
        try
        {
            var docRef = _database.Collection(_appOptions.Collection.User).Document(user.Id);
            var userEntity = _mapper.Map<UserEntity>(user);
            await docRef.SetAsync(userEntity);
            return new ResponseVm { IsSuccess = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating profile: {ex.Message}");
            return new ResponseVm { IsSuccess = false, ErrorMessage = "Failed to update profile." };
        }
    }

    public async Task<ResponseVm> UpdatedUserInfo(UserModel user)
    {
        try
        {
            var users = _database.Collection(_appOptions.Collection.User);
            var query = users.WhereEqualTo("EmailAddress", user.EmailAddress.Trim().ToLower());
            var querySnapshot = await query.GetSnapshotAsync();

            if (!querySnapshot.Any())
            {
                return new ResponseVm { IsSuccess = false, ErrorMessage = "User not found." };
            }

            var userEntity = querySnapshot.First().ConvertTo<UserEntity>();

            if (!string.IsNullOrEmpty(user.CardsCompleted))
            {
                userEntity.CardsCompleted = user.CardsCompleted;
                if (int.TryParse(user.CardsCompleted, out int cards) && cards >= 52)
                {
                    userEntity.HasCompletedChallenge = true;
                }
            }

            if (!string.IsNullOrEmpty(user.TimeCompleted))
            {
                userEntity.TimeCompleted = user.TimeCompleted;
            }

            if (string.IsNullOrEmpty(userEntity.CompletedBy))
            {
                userEntity.CompletedBy = DateTime.UtcNow.ToString("O");
            }

            if (!string.IsNullOrEmpty(user.Review) && string.IsNullOrEmpty(userEntity.Review))
            {
                userEntity.Review = user.Review;
            }

            var docRef = _database.Collection(_appOptions.Collection.User).Document(userEntity.Id);
            await docRef.SetAsync(userEntity);

            return new ResponseVm { IsSuccess = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update user error: {ex.Message}");
            return new ResponseVm { IsSuccess = false, ErrorMessage = "Failed to update." };
        }
    }

    public async Task<ResponseVm> SaveWorkoutSession(WorkoutSession session)
    {
        try
        {
            var sessionEntity = new WorkoutSessionEntity
            {
                Id = session.Id,
                UserId = session.UserId,
                UserEmail = session.UserEmail,
                CardsCompleted = session.CardsCompleted,
                TimeElapsed = session.TimeElapsed,
                StartedAt = session.StartedAt.ToString("O"),
                CompletedAt = session.CompletedAt?.ToString("O") ?? "",
                IsCompleted = session.IsCompleted,
                CardSequence = session.CardSequence
            };

            var docRef = _database.Collection(_appOptions.Collection.WorkoutSession).Document(session.Id);
            await docRef.SetAsync(sessionEntity);

            return new ResponseVm { IsSuccess = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving session: {ex.Message}");
            return new ResponseVm { IsSuccess = false };
        }
    }

    public async Task<List<WorkoutSession>> GetUserWorkoutHistory(string userId)
    {
        try
        {
            var sessions = _database.Collection(_appOptions.Collection.WorkoutSession);
            var query = sessions.WhereEqualTo("UserId", userId);
            var snapshot = await query.GetSnapshotAsync();

            var workoutSessions = new List<WorkoutSession>();

            foreach (var doc in snapshot.Documents)
            {
                var entity = doc.ConvertTo<WorkoutSessionEntity>();
                workoutSessions.Add(new WorkoutSession
                {
                    Id = entity.Id,
                    UserId = entity.UserId,
                    UserEmail = entity.UserEmail,
                    CardsCompleted = entity.CardsCompleted,
                    TimeElapsed = entity.TimeElapsed,
                    StartedAt = DateTime.Parse(entity.StartedAt),
                    CompletedAt = string.IsNullOrEmpty(entity.CompletedAt) ? null : DateTime.Parse(entity.CompletedAt),
                    IsCompleted = entity.IsCompleted,
                    CardSequence = entity.CardSequence
                });
            }

            return workoutSessions.OrderByDescending(w => w.StartedAt).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting history: {ex.Message}");
            return new List<WorkoutSession>();
        }
    }

    public async Task<UserStats?> GetUserStats(string userId)
    {
        try
        {
            var user = await GetUserById(userId);
            if (user == null) return null;

            var workouts = await GetUserWorkoutHistory(userId);
            var completed = workouts.Where(w => w.IsCompleted).ToList();

            var stats = new UserStats
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Email = user.EmailAddress,
                TotalWorkouts = completed.Count,
                TotalCardsCompleted = completed.Sum(w => w.CardsCompleted),
                RecentWorkouts = workouts.Take(5).ToList(),
                HasCompleted52Cards = user.HasCompletedChallenge,
                MemberSince = DateTime.Parse(user.CreatedBy),
                LastWorkoutDate = completed.Any() ? completed.First().CompletedAt : null
            };

            if (completed.Any())
            {
                var best = completed.OrderBy(w => ParseTime(w.TimeElapsed)).First();
                stats.BestTime = best.TimeElapsed;
            }

            return stats;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting stats: {ex.Message}");
            return null;
        }
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboard(int topN = 10)
    {
        return await GetLeaderboardByMostWorkouts(topN);
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboardByBestTime(int topN = 10)
    {
        try
        {
            var users = await GetUsers();
            var leaderboard = new List<LeaderboardEntry>();

            foreach (var user in users)
            {
                var workouts = await GetUserWorkoutHistory(user.Id);
                var completed = workouts.Where(w => w.IsCompleted && w.CardsCompleted == 52).ToList();

                if (completed.Any())
                {
                    var best = completed.OrderBy(w => ParseTime(w.TimeElapsed)).First();
                    leaderboard.Add(new LeaderboardEntry
                    {
                        UserId = user.Id,
                        UserName = $"{user.FirstName} {user.LastName}",
                        TotalWorkouts = completed.Count,
                        TotalCardsCompleted = workouts.Sum(w => w.CardsCompleted),
                        BestTime = best.TimeElapsed,
                        BestTimeInMilliseconds = ParseTime(best.TimeElapsed),
                        LastWorkoutDate = completed.First().CompletedAt,
                        HasCompleted52Cards = true
                    });
                }
            }

            return leaderboard.OrderBy(l => l.BestTimeInMilliseconds).Take(topN).ToList();
        }
        catch
        {
            return new List<LeaderboardEntry>();
        }
    }

    public async Task<List<LeaderboardEntry>> GetLeaderboardByMostWorkouts(int topN = 10)
    {
        try
        {
            var users = await GetUsers();
            var leaderboard = new List<LeaderboardEntry>();

            foreach (var user in users)
            {
                var workouts = await GetUserWorkoutHistory(user.Id);
                var completed = workouts.Where(w => w.IsCompleted).ToList();

                if (completed.Any())
                {
                    var best = completed.OrderBy(w => ParseTime(w.TimeElapsed)).FirstOrDefault();
                    leaderboard.Add(new LeaderboardEntry
                    {
                        UserId = user.Id,
                        UserName = $"{user.FirstName} {user.LastName}",
                        TotalWorkouts = completed.Count,
                        TotalCardsCompleted = workouts.Sum(w => w.CardsCompleted),
                        BestTime = best?.TimeElapsed ?? "N/A",
                        BestTimeInMilliseconds = best != null ? ParseTime(best.TimeElapsed) : int.MaxValue,
                        LastWorkoutDate = completed.First().CompletedAt,
                        HasCompleted52Cards = user.HasCompletedChallenge
                    });
                }
            }

            return leaderboard.OrderByDescending(l => l.TotalWorkouts).Take(topN).ToList();
        }
        catch
        {
            return new List<LeaderboardEntry>();
        }
    }

    private int ParseTime(string time)
    {
        try
        {
            time = time.Replace("m", "").Replace("s", "").Replace("ms", "").Replace(" ", "");
            var parts = time.Split(':');
            if (parts.Length == 3)
            {
                return (int.Parse(parts[0]) * 60000) + (int.Parse(parts[1]) * 1000) + int.Parse(parts[2]);
            }
            return int.MaxValue;
        }
        catch
        {
            return int.MaxValue;
        }
    }
}