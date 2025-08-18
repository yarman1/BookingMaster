using AutoMapper;
using BookingMaster.Constants;
using BookingMaster.Data;
using BookingMaster.Dtos.Feedbacks;
using BookingMaster.Interfaces;
using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingMaster.Services
{
    public class FeedbackService(ApplicationDBContext dbContext, IMapper mapper) : IFeedbackService
    {
        private readonly ApplicationDBContext _dbContext = dbContext;

        private readonly IMapper _mapper = mapper;

        public async Task<Result<FeedbackResponse>> CreateFeedbackAsync(FeedbackCreateRequest request, int customerId)
        {
            var booking = await _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Feedback)
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.CustomerId == customerId);

            if (booking == null)
                return new Result<FeedbackResponse>(false, null, "Booking not found or does not belong to this customer");

            if (booking.Status.NormalizedName != Statuses.Confirmed)
                return new Result<FeedbackResponse>(false, null, "Feedback can only be added for confirmed bookings");

            if (booking.Feedback != null)
                return new Result<FeedbackResponse>(false, null, "Feedback already exists for this booking");

            Feedback feedback = new()
            {
                BookingId = booking.Id,
                Comment = request.Comment,
                Rating = request.Rating,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Booking = booking,
            };

            // Calculate accommodation ratings
            using var transaction = await _dbContext.Database.BeginTransactionAsync();


            await _dbContext.Feedback.AddAsync(feedback);
            await _dbContext.SaveChangesAsync();

            var accommodation = booking.AccommodationProposal.Accommodation;
            accommodation.FeedbackCount++;
            accommodation.TotalRating += request.Rating;
            accommodation.AverageRating = (double)accommodation.TotalRating / accommodation.FeedbackCount;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = _mapper.Map<FeedbackResponse>(feedback);
            return new Result<FeedbackResponse>(true, response, null);
        }

        public async Task<Result<IEnumerable<FeedbackResponse>>> GetFeedbacksByAccommodationAsync(int accommodationId)
        {
            var accommodation = await _dbContext.Accommodation
                .FirstOrDefaultAsync(a => a.Id == accommodationId);

            if (accommodation == null)
                return new Result<IEnumerable<FeedbackResponse>>(false, null, "Accommodation not found");

            var feedbacks = await _dbContext.Feedback
                 .Include(f => f.Booking)
                    .ThenInclude(b => b.Customer)
                 .Include(f => f.Booking)
                    .ThenInclude(b => b.AccommodationProposal)
                .Where(f => f.Booking.AccommodationProposal.AccommodationId == accommodationId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var response = feedbacks.Select(f => _mapper.Map<FeedbackResponse>(f)).ToList();
            return new Result<IEnumerable<FeedbackResponse>>(true, response, null);
        }

        public async Task<Result<FeedbackResponse>> UpdateFeedbackAsync(int feedbackId, FeedbackUpdateRequest request, int customerId)
        {
            var feedback = await _dbContext.Feedback
                .Include(f => f.Booking)
                .ThenInclude(b => b.AccommodationProposal)
                .ThenInclude(p => p.Accommodation)
                .FirstOrDefaultAsync(f => f.Id == feedbackId && f.Booking.CustomerId == customerId);

            if (feedback == null)
                return new Result<FeedbackResponse>(false, null, "Feedback not found or does not belong to this customer");

            var accommodation = feedback.Booking.AccommodationProposal.Accommodation;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            accommodation.TotalRating -= feedback.Rating;

            if (!string.IsNullOrWhiteSpace(request.Comment))
                feedback.Comment = request.Comment;

            if (request.Rating.HasValue)
            {
                feedback.Rating = request.Rating.Value;
                accommodation.TotalRating += request.Rating.Value;
            }
            else
            {
                accommodation.TotalRating += feedback.Rating;
            }

            accommodation.AverageRating = accommodation.FeedbackCount > 0
                ? (double)accommodation.TotalRating / accommodation.FeedbackCount
                : 0;

            feedback.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = _mapper.Map<FeedbackResponse>(feedback);
            return new Result<FeedbackResponse>(true, response, null);
        }

        public async Task<Result<bool>> DeleteFeedbackAsync(int feedbackId, int customerId)
        {
            var feedback = await _dbContext.Feedback
                .Include(f => f.Booking)
                .ThenInclude(b => b.AccommodationProposal)
                .ThenInclude(p => p.Accommodation)
                .FirstOrDefaultAsync(f => f.Id == feedbackId && f.Booking.CustomerId == customerId);

            if (feedback == null)
                return new Result<bool>(false, false, "Feedback not found or does not belong to this customer");

            var accommodation = feedback.Booking.AccommodationProposal.Accommodation;

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            accommodation.TotalRating -= feedback.Rating;
            accommodation.FeedbackCount = Math.Max(0, accommodation.FeedbackCount - 1);
            accommodation.AverageRating = accommodation.FeedbackCount > 0
                ? (double)accommodation.TotalRating / accommodation.FeedbackCount
                : 0;

            _dbContext.Feedback.Remove(feedback);

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Result<bool>(true, true, null);
        }
    }
}
