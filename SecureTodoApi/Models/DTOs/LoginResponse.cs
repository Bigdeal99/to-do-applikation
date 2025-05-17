namespace SecureTodoApi.Models.DTOs
{
    public class LoginResponse
    {
        public required string Username { get; set; }
        public string Token { get; set; }
    }
}
