using BookingMaster.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.AccommodationProposal
{
    public class AccommodationProposalCreateRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name must be between 4 and 30 characters")]
        public required string Name { get; set; }

        [Required]
        [Range(1, 50, ErrorMessage = "GuestCount must be between 1 and 50")]
        public required int GuestCount { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "PricePerNight must be between 0.01 and 999999.99")]
        [TwoDecimalPlaces]
        public required decimal PricePerNight { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public required int Quantity { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "AccommodationId must be a valid ID")]
        public required int AccommodationId { get; set; }
    }
}
