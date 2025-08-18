using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Feedbacks
{
    public class FeedbackUpdateRequest
    {
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 200 characters")]
        public string? Comment { get; set; } = string.Empty;

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public int? Rating { get; set; }
    }
}
