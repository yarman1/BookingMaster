using BookingMaster.Dtos.Feedbacks;

namespace BookingMaster.Interfaces
{
    public interface IFeedbackService
    {
        Task<Result<FeedbackResponse>> CreateFeedbackAsync(FeedbackCreateRequest request, int customerId);
        Task<Result<IEnumerable<FeedbackResponse>>> GetFeedbacksByAccommodationAsync(int accommodationId);
        Task<Result<FeedbackResponse>> UpdateFeedbackAsync (int feedbackId, FeedbackUpdateRequest request, int customerId);
        Task<Result<bool>> DeleteFeedbackAsync(int feedbackId, int customerId);
    }
}
