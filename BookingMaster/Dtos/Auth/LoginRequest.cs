using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }   
    }
}
