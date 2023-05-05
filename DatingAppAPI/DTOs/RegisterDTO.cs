using System.ComponentModel.DataAnnotations;

namespace DatingAppAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; }
    }
}