namespace SecureTodoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }= string.Empty;
        public string PasswordHash { get; set; }= string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public List<TodoItem> TodoItems { get; set; }= null!;
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEndTime { get; set; }

    }
}
