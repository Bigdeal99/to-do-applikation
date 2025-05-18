using SecureTodoApi.Models;
using SecureTodoApi.Repositories;
using SecureTodoApi.Security;
using SecureTodoApi.Models.DTOs;

namespace SecureTodoApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly PasswordHasher _hasher;

        public UserService(IUserRepository userRepo, PasswordHasher hasher)
        {
            _userRepo = userRepo;
            _hasher = hasher;
        }

        public User Register(string username, string password)
        {
            if (_userRepo.GetByUsername(username) != null)
                return null;

            var user = new User
            {
                Username = username,
                PasswordHash = _hasher.Hash(password)
            };

            _userRepo.Create(user);
            return user;
        }
        public User? GetByUsername(string username)
{
    return _userRepo.GetByUsername(username);
}

        public AuthResult ValidateUser(string username, string password)

        {
            var user = _userRepo.GetByUsername(username);
            if (user == null)
                return new AuthResult { IsSuccess = false };

            if (user.LockoutEndTime.HasValue && user.LockoutEndTime > DateTime.UtcNow)
            {
                return new AuthResult { IsSuccess = false, IsLockedOut = true, User = user };
            }

            if (!_hasher.Verify(password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEndTime = DateTime.UtcNow.AddMinutes(15);
                }

                _userRepo.Update(user);
                return new AuthResult { IsSuccess = false };
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEndTime = null;
            _userRepo.Update(user);

            return new AuthResult { IsSuccess = true, User = user };
        }

        public User? GetByRefreshToken(string token)
        {
            return _userRepo.GetAll().FirstOrDefault(u => u.RefreshToken == token);
        }
        public void UpdateUser(User user)
        {
            _userRepo.Update(user);
        }


    }
}
