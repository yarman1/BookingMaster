using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Auth
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Name must be between 4 and 25 characters")]
        public required string FirstName { get; set; }
        
        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "Last name must be between 4 and 25 characters")]
        public required string LastName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(
        @"^(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{12,}$",
        ErrorMessage = "Password must be at least 12 characters long and contain at least one uppercase letter, one number, and one special character"
        )]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"^\+380\d{9}$",
        ErrorMessage = "Phone must be in format +380XXXXXXXXX"
        )]
        public required string Phone { get; set; }
    }
}
