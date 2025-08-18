using BookingMaster.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.AccommodationProposal
{
    public class AccommodationProposalUpdateRequest
    {
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Name must be between 4 and 30 characters")]
        public string? Name { get; set; }

        [Range(1, 50, ErrorMessage = "GuestCount must be between 1 and 50")]
        public int? GuestCount { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "PricePerNight must be between 0.01 and 999999.99")]
        [TwoDecimalPlaces]
        public decimal? PricePerNight { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int? Quantity { get; set; }

        public bool? IsActive { get; set; }
    }
}
