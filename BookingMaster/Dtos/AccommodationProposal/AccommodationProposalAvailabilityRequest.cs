using BookingMaster.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.AccommodationProposal
{
    public class AccommodationProposalAvailabilityRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "AccommodationId must be a valid ID")]
        public required int AccommodationId { get; set; }

        [Required]
        public required DateTime StartDate { get; set; }

        [Required]
        [DateGreaterThan("StartDate", ErrorMessage = "EndDate must be greater than StartDate")]
        public required DateTime EndDate { get; set; }

        [Range(1, 50, ErrorMessage = "GuestCount must be between 1 and 50")]
        public int? GuestCount { get; set; }
    }
}
