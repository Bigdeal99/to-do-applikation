using SecureTodoApi.Models;

namespace SecureTodoApi.Repositories
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        void Create(User user);
    }
}
