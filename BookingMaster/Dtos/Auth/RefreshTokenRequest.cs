using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Auth
{
    public class RefreshTokenRequest
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
}
