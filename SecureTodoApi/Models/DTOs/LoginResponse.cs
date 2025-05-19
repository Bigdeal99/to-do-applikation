namespace SecureTodoApi.Models.DTOs
{
    public class LoginResponse
    {
        public required string Username { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
