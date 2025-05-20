using SecureTodoApi.Models.DTOs;

namespace SecureTodoApi.Services

{
    using SecureTodoApi.Models;

    public interface IUserService
    {
        (User? User, string[]? Errors) Register(string username, string password);
        AuthResult ValidateUser(string username, string password);

        User? GetByRefreshToken(string token);
        void UpdateUser(User user);
        User? GetByUsername(string username);

    }
}
