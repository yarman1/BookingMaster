using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public required string Token { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime Expiration { get; set; }

        public int UserId { get; set; }

        public required User User { get; set; }

        public bool IsActive => DateTime.UtcNow < Expiration;
    }
}
