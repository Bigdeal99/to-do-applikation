namespace SecureTodoApi.Services
{
    using SecureTodoApi.Models;

    public interface IUserService
    {
        User Register(string username, string password);
        User ValidateUser(string username, string password);
        User? GetByRefreshToken(string token);
        void UpdateUser(User user);
    }
}
