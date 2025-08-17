using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Comment { get; set; }

        public int CustomerId { get; set; }

        public required User Customer { get; set; }

        public int AccommodationProposalId { get; set; }

        public required AccommodationProposal AccommodationProposal { get; set; }

        public int StatusId { get; set; }

        public required BookingStatus Status { get; set; }

        public Feedback? Feedback { get; set; }
    }
}
