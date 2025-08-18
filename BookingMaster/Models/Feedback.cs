using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Comment { get; set; } = string.Empty;

        [Column(TypeName = "int")]
        public int Rating { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2(0)")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int BookingId { get; set; }

        public required Booking Booking { get; set; }
    }
}
