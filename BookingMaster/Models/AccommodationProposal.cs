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

        public int AccommodationId { get; set; }

        public required Accommodation Accommodation { get; set; }
    }
}
