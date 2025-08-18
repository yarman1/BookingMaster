using AutoMapper;
using BookingMaster.Constants;
using BookingMaster.Data;
using BookingMaster.Dtos.Bookings;
using BookingMaster.Interfaces;
using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingMaster.Services
{
    public class BookingService(ApplicationDBContext dbContext, IMapper mapper) : IBookingService
    {
        private readonly ApplicationDBContext _dbContext = dbContext;

        private readonly IMapper _mapper = mapper;

        public async Task<Result<BookingCustomerResponse>> CreateBookingAsync(BookingCreateRequest request, int customerId)
        {
            User? customer = await _dbContext.User.FindAsync(customerId);
            if (customer == null)
                return new Result<BookingCustomerResponse>(false, null, "Customer not found");

            var proposal = await _dbContext.AccommodationProposal
                .Include(p => p.Bookings)
                    .ThenInclude(b => b.Status)
                .FirstOrDefaultAsync(p => p.Id == request.AccommodationProposalId && p.IsActive);

            if (proposal == null)
                return new Result<BookingCustomerResponse>(false, null, "Accommodation proposal not found or inactive");

            int numberOfNights = (request.EndDate.Date - request.StartDate.Date).Days;

            int overlappingBookings = proposal.Bookings
                .Count(b => b.StartDate < request.EndDate &&
                            b.EndDate > request.StartDate &&
                            b.Status.Name != "Cancelled");

            if (overlappingBookings >= proposal.Quantity)
                return new Result<BookingCustomerResponse>(false, null, "No available rooms for the selected dates");

            var initialStatus = await _dbContext.BookingStatus
                .FirstOrDefaultAsync(s => s.NormalizedName == Statuses.Pending);

            if (initialStatus == null)
                return new Result<BookingCustomerResponse>(false, null, "Booking status 'Pending' not found");

            var booking = new Booking
            {
                AccommodationProposalId = request.AccommodationProposalId,
                AccommodationProposal = proposal,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Price = proposal.PricePerNight * numberOfNights,
                Comment = request.Comment,
                CustomerId = customerId,
                Customer = customer,
                StatusId = initialStatus.Id,
                Status = initialStatus,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _dbContext.Booking.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<BookingCustomerResponse>(booking);
            return new Result<BookingCustomerResponse>(true, response, null);
        }

        public async Task<Result<BookingCustomerResponse>> GetBookingByIdCustomerAsync(int bookingId, int customerId)
        {
            var booking = await _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Customer)
                .Include(b => b.Feedback)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.CustomerId == customerId);

            if (booking == null)
            {
                return new Result<BookingCustomerResponse>(false, null, "Booking not found or does not belong to this customer");
            }

            var response = _mapper.Map<BookingCustomerResponse>(booking);
            return new Result<BookingCustomerResponse>(true, response, null);
        }

        public async Task<Result<IEnumerable<BookingCustomerResponse>>> GetBookingsByCustomerAsync(BookingQuery query, int customerId)
        {
            var bookingsQuery = _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                    .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Customer)
                .Include(b => b.Feedback)
                .Where(b => b.CustomerId == customerId);

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                string normalizedStatus = query.Status.Trim().ToUpperInvariant();
                bookingsQuery = bookingsQuery.Where(b => b.Status.NormalizedName == normalizedStatus);
            }

            var bookings = await bookingsQuery
                .OrderByDescending(b => b.UpdatedAt)
                .ToListAsync();

            var responses = bookings.Select(b => _mapper.Map<BookingCustomerResponse>(b)).ToList();

            return new Result<IEnumerable<BookingCustomerResponse>>(true, responses, null);
        }

        public async Task<Result<BookingOwnerResponse>> GetBookingByIdOwnerAsync(int bookingId, int ownerId)
        {
            var booking = await _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                    .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Customer)
                .Include(b => b.Feedback)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
            {
                return new Result<BookingOwnerResponse>(false, null, "Booking not found");
            }

            if (booking.AccommodationProposal.Accommodation.OwnerId != ownerId)
            {
                return new Result<BookingOwnerResponse>(false, null, "You do not have permission to view this booking");
            }

            var response = _mapper.Map<BookingOwnerResponse>(booking);
            return new Result<BookingOwnerResponse>(true, response, null);
        }

        public async Task<Result<IEnumerable<BookingOwnerResponse>>> GetBookingsByOwnerAsync(BookingQuery query, int ownerId)
        {
            string? normalizedStatus = null;
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                normalizedStatus = query.Status.Trim().ToUpperInvariant();
            }

            var bookingsQuery = _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                    .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Customer)
                .Include(b => b.Feedback)
                .Where(b => b.AccommodationProposal.Accommodation.OwnerId == ownerId);

            if (normalizedStatus != null)
            {
                bookingsQuery = bookingsQuery.Where(b => b.Status.NormalizedName == normalizedStatus);
            }

            var bookings = await bookingsQuery.OrderByDescending(b => b.UpdatedAt).ToListAsync();

            var responses = bookings.Select(b => _mapper.Map<BookingOwnerResponse>(b)).ToList();

            return new Result<IEnumerable<BookingOwnerResponse>>(true, responses, null);
        }

        public async Task<Result<BookingOwnerResponse>> ChangeBookingStatus(int bookingId, int ownerId, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                return new Result<BookingOwnerResponse>(false, null, "New status is required");

            string normalizedStatus = newStatus.Trim().ToUpperInvariant();

            var booking = await _dbContext.Booking
                .Include(b => b.AccommodationProposal)
                    .ThenInclude(p => p.Accommodation)
                .Include(b => b.Status)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return new Result<BookingOwnerResponse>(false, null, "Booking not found");

            if (booking.AccommodationProposal.Accommodation.OwnerId != ownerId)
                return new Result<BookingOwnerResponse>(false, null, "You do not have permission to change this booking");

            string currentStatus = booking.Status.NormalizedName;
            if (currentStatus == Statuses.Confirmed || currentStatus == Statuses.Cancelled)
                return new Result<BookingOwnerResponse>(false, null, $"Cannot change booking status because it is already '{booking.Status.Name}'");

            var status = await _dbContext.BookingStatus
                .FirstOrDefaultAsync(s => s.NormalizedName == normalizedStatus);

            if (status == null)
                return new Result<BookingOwnerResponse>(false, null, $"Booking status '{newStatus}' not found");

            booking.StatusId = status.Id;
            booking.Status = status;
            booking.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<BookingOwnerResponse>(booking);
            return new Result<BookingOwnerResponse>(true, response, null);
        }
    }
}   
