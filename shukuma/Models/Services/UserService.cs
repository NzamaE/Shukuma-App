//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using shukuma.Models.Interfaces;
//using shukuma.persistence.sqlserver;
//using shukuma.Models.ViewModels;

//namespace shukuma.Models.Services
//{
//    public class UserService : IUserService
//    {
//        private readonly IMapper _mapper;
//        private readonly UserDbContext _context;
//        private readonly IHashService _hashService;

//        public UserService(UserDbContext context, IHashService hashService)
//        {
//            var mappingConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
//            _mapper = new Mapper(mappingConfig);

//            _context = context;
//            _hashService = hashService;
//        }

//        public async Task<ResponseVm> SignIn(SigninModel user)
//        {
//            try
//            {
//                var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == user.EmailAddress);
//                if (userEntity is null)
//                    return new ResponseVm
//                    {
//                        IsSuccess = false,
//                        ErrorMessage = $"Could not sign you in. No profile is linked to {user.EmailAddress}."
//                    };

//                var isPasswordCorrect = userEntity.Password == _hashService.GetHash(user.Password);
//                if (!isPasswordCorrect)
//                    return new ResponseVm
//                    {
//                        IsSuccess = false,
//                        ErrorMessage = $"Could not sign you in. The provided password is incorrect for {user.EmailAddress}."
//                    };

//                return new ResponseVm
//                {
//                    IsSuccess = true,
//                    User = _mapper.Map<UserModel>(userEntity)
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ResponseVm
//                {
//                    IsSuccess = false,
//                    ErrorMessage = $"Could not sign you in. {ex.Message}. Support is looking into it. Please try again later."
//                };
//            }
//        }

//        public async Task<ResponseVm> SignUp(SignupModel user)
//        {
//            try
//            {
//                if (_context.Users.Any(x => x.EmailAddress == user.EmailAddress))
//                    return new ResponseVm 
//                    { 
//                        IsSuccess = false, 
//                        ErrorMessage = $"Could not register you. {user.EmailAddress} is already linked to another Profile."
//                    };

//                var userEntity = _mapper.Map<UserEntity>(user);
//                userEntity.Password = _hashService.GetHash(userEntity.Password);

//                await _context.Users.AddAsync(userEntity);
//                var result = _context.SaveChanges();

//                return new ResponseVm
//                {
//                    IsSuccess = result > 0,
//                    ErrorMessage = result > 0 ? "" : $"Could not register you with {user.EmailAddress}. Support is looking into it. Please try again later."
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ResponseVm
//                {
//                    IsSuccess = false,
//                    ErrorMessage = $"Profile could not be registered. {ex.Message}. Support is looking into it. Please try again later."
//                };
//            }
//        }

//        public async Task<ResponseVm> UpdatedUserInfo(UserModel user)
//        {
//            try
//            {
//                var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.EmailAddress == user.EmailAddress);
//                if (userEntity is null)
//                    return new ResponseVm
//                    {
//                        IsSuccess = false,
//                        ErrorMessage = "Could not update profile. Support is looking into it."
//                    };

//                if (int.Parse(user.CardsCompleted) == 52)
//                    userEntity.HasCompletedChallenge = true;

//                if (userEntity.CompletedBy is null) userEntity.CompletedBy = DateTime.Now;
//                userEntity.CardsCompleted = user.CardsCompleted;
//                userEntity.TimeCompleted = user.TimeCompleted;

//                if (userEntity.Review is null) userEntity.Review = user.Review;

//                _context.Update(userEntity);
//                var result = _context.SaveChanges();

//                return new ResponseVm
//                {
//                    IsSuccess = result > 0,
//                    ErrorMessage = result > 0 ? "" : "Could not update profile. Support is looking into it. Please try again later."
//                };
//            }
//            catch (Exception ex)
//            {
//                return new ResponseVm
//                {
//                    IsSuccess = false,
//                    ErrorMessage = $"Could not update profile. {ex.Message}. Support is looking into it. Please try again later."
//                };
//            }
//        }
//    }
//}
