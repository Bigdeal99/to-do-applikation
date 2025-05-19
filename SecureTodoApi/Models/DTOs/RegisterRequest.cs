using System.ComponentModel.DataAnnotations;
namespace SecureTodoApi.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(4)]
        public required string Username { get; set; }

        [Required]
        [MinLength(6)]
        public required  string Password { get; set; }
    }
}
