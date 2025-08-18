using BookingMaster.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Bookings
{
    public class BookingCreateRequest
    {
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DateGreaterThan("StartDate", ErrorMessage = "EndDate must be greater than StartDate")]
        public DateTime EndDate { get; set; }

        [StringLength(200, ErrorMessage = "Comment cannot exceed 200 characters")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "AccommodationProposalId must be a valid ID")]
        public int AccommodationProposalId { get; set; }
    }
}
