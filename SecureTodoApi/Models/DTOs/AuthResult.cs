
namespace SecureTodoApi.Models.DTOs
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public bool IsLockedOut { get; set; }
        public User? User { get; set; }
    }
}