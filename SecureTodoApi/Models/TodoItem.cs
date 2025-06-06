namespace SecureTodoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }= string.Empty;
        public bool IsCompleted { get; set; }
         public string? Description { get; set; } 
         public string? Category { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }

    }
}
