using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Feedbacks
{
    public class FeedbackCreateRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 200 characters")]
        public required string Comment { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public required int Rating { get; set; }

        [Required(ErrorMessage = "BookingId must be a valid ID")]
        public required int BookingId { get; set; }
    }
}
