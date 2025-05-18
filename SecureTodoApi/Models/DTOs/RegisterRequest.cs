using System.ComponentModel.DataAnnotations;
namespace SecureTodoApi.Models.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
