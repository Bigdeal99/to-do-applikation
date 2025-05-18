using System.ComponentModel.DataAnnotations;


namespace SecureTodoApi.Models.DTOs
{
    public class TodoCreateRequest
    {

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
