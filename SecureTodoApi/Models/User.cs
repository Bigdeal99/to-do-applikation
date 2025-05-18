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

    }
}
