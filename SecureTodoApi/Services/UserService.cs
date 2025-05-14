using SecureTodoApi.Models;
using SecureTodoApi.Repositories;
using SecureTodoApi.Security;

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

        public User ValidateUser(string username, string password)
        {
            var user = _userRepo.GetByUsername(username);
            if (user == null) return null;

            return _hasher.Verify(password, user.PasswordHash) ? user : null;
        }
    }
}
