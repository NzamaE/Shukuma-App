using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using shukuma.domain.Interfaces;
using shukuma.domain.Models;
using shukuma.persistence.firebase;

namespace shukuma.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = new Mapper(mappingConfig);
        }

        #region Authentication

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupModel userModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.SignUp(userModel);
                if (response.IsSuccess)
                {
                    // Get the full user info including ID
                    var userInfo = await _userService.GetUserByEmail(userModel.EmailAddress);
                    if (userInfo != null)
                    {
                        var user = _mapper.Map<UserModel>(userInfo);
                        SetUserSession(user);
                        return RedirectToAction("Dashboard");
                    }
                }
                ViewData["Error"] = response.ErrorMessage;
                return View();
            }
            ViewData["Error"] = "User data is invalid. Please validate input data.";
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninModel userModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _userService.SignIn(userModel);
                if (response.IsSuccess && response.User != null)
                {
                    // Get the full user info including ID
                    var userInfo = await _userService.GetUserByEmail(userModel.EmailAddress);
                    if (userInfo != null)
                    {
                        var user = _mapper.Map<UserModel>(userInfo);
                        SetUserSession(user);
                        return RedirectToAction("Dashboard");
                    }
                }
                ViewData["Error"] = response.ErrorMessage;
                return View();
            }
            ViewData["Error"] = "User data is invalid. Please validate input data.";
            return View();
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }

        #endregion

        #region Dashboard & Profile

        public async Task<IActionResult> Dashboard()
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            var userInfo = await _userService.GetUserByEmail(user.EmailAddress);
            if (userInfo == null)
                return RedirectToAction("SignIn");

            var stats = await _userService.GetUserStats(userInfo.Id);
            ViewData["UserStats"] = stats;

            return View(user);
        }

        public async Task<IActionResult> Profile()
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            var userInfo = await _userService.GetUserByEmail(user.EmailAddress);
            if (userInfo == null)
                return RedirectToAction("SignIn");

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UserInfo userInfo)
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            var response = await _userService.UpdateUserProfile(userInfo);

            if (response.IsSuccess)
            {
                ViewData["Success"] = "Profile updated successfully!";

                // Update session with new data
                var updatedUser = _mapper.Map<UserModel>(userInfo);
                SetUserSession(updatedUser);
            }
            else
            {
                ViewData["Error"] = response.ErrorMessage;
            }

            return View("Profile", userInfo);
        }

        #endregion

        #region Workout

        public IActionResult Exercise()
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Completed(UserModel userModel)
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            // Get full user info
            var userInfo = await _userService.GetUserByEmail(userModel.EmailAddress);
            if (userInfo == null)
            {
                ViewData["Error"] = "User not found.";
                return View(userModel);
            }

            // Save workout session FIRST
            var session = new WorkoutSession
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userInfo.Id,
                UserEmail = userModel.EmailAddress,
                CardsCompleted = int.TryParse(userModel.CardsCompleted, out int cards) ? cards : 0,
                TimeElapsed = userModel.TimeCompleted ?? "00:00:00",
                StartedAt = DateTime.UtcNow.AddMinutes(-10), // Approximate start time
                CompletedAt = DateTime.UtcNow,
                IsCompleted = true
            };

            var sessionResponse = await _userService.SaveWorkoutSession(session);
            if (!sessionResponse.IsSuccess)
            {
                Console.WriteLine($"Failed to save workout session: {sessionResponse.ErrorMessage}");
            }

            // Update user info
            var response = await _userService.UpdatedUserInfo(userModel);

            if (response.IsSuccess)
            {
                // Calculate progress
                var progress = Math.Round((int.Parse(userModel.CardsCompleted ?? "0") / 52m) * 100);
                ViewData["ProgressValue"] = $"{progress}%";

                var progressClass = "progress-bar progress-bar-striped progress-bar-animated";
                if (progress < 25)
                    progressClass += " bg-danger";
                else if (progress < 50)
                    progressClass += " bg-warning";
                else if (progress < 75)
                    progressClass += " bg-info";
                else
                    progressClass += " bg-success";

                ViewData["ProgressClass"] = progressClass;
                ViewData["Review"] = userModel.Review;

                return View(userModel);
            }
            else
            {
                ViewData["Error"] = response.ErrorMessage;
                return View(userModel);
            }
        }

        #endregion

        #region Workout History

        public async Task<IActionResult> WorkoutHistory()
        {
            var user = GetUserFromSession();
            if (user == null)
                return RedirectToAction("SignIn");

            var userInfo = await _userService.GetUserByEmail(user.EmailAddress);
            if (userInfo == null)
                return RedirectToAction("SignIn");

            var history = await _userService.GetUserWorkoutHistory(userInfo.Id);
            ViewData["UserName"] = $"{userInfo.FirstName} {userInfo.LastName}";

            return View(history);
        }

        #endregion

        #region Leaderboard

        public async Task<IActionResult> Leaderboard()
        {
            var leaderboard = await _userService.GetLeaderboard(20);
            return View(leaderboard);
        }

        public async Task<IActionResult> LeaderboardByTime()
        {
            var leaderboard = await _userService.GetLeaderboardByBestTime(20);
            return View("Leaderboard", leaderboard);
        }

        public async Task<IActionResult> LeaderboardByWorkouts()
        {
            var leaderboard = await _userService.GetLeaderboardByMostWorkouts(20);
            return View("Leaderboard", leaderboard);
        }

        #endregion

        #region Session Management Helpers

        private void SetUserSession(UserModel user)
        {
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.EmailAddress);
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserLastName", user.LastName);
            HttpContext.Session.SetString("UserName", user.Username);

            Console.WriteLine($"Session set for user: {user.Id} - {user.EmailAddress}");
        }

        private UserModel? GetUserFromSession()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("No user session found");
                return null;
            }

            Console.WriteLine($"Retrieved session for user: {userId} - {userEmail}");

            return new UserModel
            {
                Id = userId,
                EmailAddress = userEmail,
                FirstName = HttpContext.Session.GetString("UserFirstName") ?? "",
                LastName = HttpContext.Session.GetString("UserLastName") ?? "",
                Username = HttpContext.Session.GetString("UserName") ?? ""
            };
        }

        #endregion
    }
}