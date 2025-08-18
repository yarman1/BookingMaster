using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class AccommodationProposal
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "int")]
        public int GuestCount { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [Column(TypeName = "int")]
        public int Quantity { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2(0)")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public int AccommodationId { get; set; }

        public required Accommodation Accommodation { get; set; }

        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
