using System.ComponentModel.DataAnnotations;

namespace SecureTodoApi.Models.DTOs
{
    public class TodoResponse
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }
        
        [MaxLength(50)]
        public string? Category { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
