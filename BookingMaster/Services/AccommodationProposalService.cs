using AutoMapper;
using BookingMaster.Constants;
using BookingMaster.Data;
using BookingMaster.Dtos.AccommodationProposal;
using BookingMaster.Interfaces;
using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingMaster.Services
{
    public class AccommodationProposalService(ApplicationDBContext dbContext, IMapper mapper) : IAccommodationProposalService
    {
        private readonly ApplicationDBContext _dbContext = dbContext;

        private readonly IMapper _mapper = mapper;

        public async Task<Result<AccommodationProposalResponse>> CreateAccommodationProposalAsync(AccommodationProposalCreateRequest request, int ownerId)
        {
            var accommodation = await _dbContext.Accommodation.FindAsync(request.AccommodationId);
            if (accommodation == null)
            {
                return new Result<AccommodationProposalResponse>(false, null, "Accommodation not found");
            }

            if (accommodation.OwnerId != ownerId)
            {
                return new Result<AccommodationProposalResponse>(false, null, "You do not have permission to create a proposal for this accommodation");
            }

            AccommodationProposal proposal = new()
            {
                AccommodationId = request.AccommodationId,
                Name = request.Name,
                GuestCount = request.GuestCount,
                PricePerNight = request.PricePerNight,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Accommodation = accommodation,
            };
            await _dbContext.AccommodationProposal.AddAsync(proposal);
            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<AccommodationProposalResponse>(proposal);
            return new Result<AccommodationProposalResponse>(true, response, null);
        }

        public async Task<Result<IEnumerable<AccommodationProposalOwnerResponse>>> GetAccommodationProposalsOwnerAsync(int accommodationId, int ownerId)
        {
            var accommodation = await _dbContext.Accommodation.FindAsync(accommodationId);
            if (accommodation == null)
            {
                return new Result<IEnumerable<AccommodationProposalOwnerResponse>>(false, null, "Accommodation not found");
            }

            if (accommodation.OwnerId != ownerId)
            {
                return new Result<IEnumerable<AccommodationProposalOwnerResponse>>(false, null, "You do not have permission to view proposals for this accommodation");
            }

            var proposals = await _dbContext.AccommodationProposal
                .Where(p => p.AccommodationId == accommodationId)
                .ToListAsync();
            var response = _mapper.Map<IEnumerable<AccommodationProposalOwnerResponse>>(proposals);
            return new Result<IEnumerable<AccommodationProposalOwnerResponse>>(true, response, null);
        }

        public async Task<Result<IEnumerable<AccommodationProposalResponse>>> GetAccommodationProposalsCustomerAsync(AccommodationProposalAvailabilityRequest request)
        {
            var accommodation = await _dbContext.Accommodation.FindAsync(request.AccommodationId);
            if (accommodation == null)
            {
                return new Result<IEnumerable<AccommodationProposalResponse>>(false, null, "Accommodation not found");
            }

            int numberOfNights = (request.EndDate.Date - request.StartDate.Date).Days;

            var proposalsWithBookings = await _dbContext.AccommodationProposal
                .Where(p => p.AccommodationId == request.AccommodationId && p.IsActive &&
                            (!request.GuestCount.HasValue || p.GuestCount >= request.GuestCount.Value))
                .Select(p => new
                {
                    Proposal = p,
                    OverlappingBookingsCount = _dbContext.Booking
                        .Count(b => b.AccommodationProposalId == p.Id &&
                                    b.StartDate < request.EndDate &&
                                    b.EndDate > request.StartDate &&
                                    b.Status.Name != Statuses.Cancelled)
                })
                .ToListAsync();

            var availableProposals = proposalsWithBookings
                .Where(p => p.OverlappingBookingsCount < p.Proposal.Quantity)
                .Select(p =>
                {
                    var response = _mapper.Map<AccommodationProposalResponse>(p.Proposal);
                    response.TotalPrice = p.Proposal.PricePerNight * numberOfNights;
                    return response;
                })
                .ToList();

            return new Result<IEnumerable<AccommodationProposalResponse>>(true, availableProposals, null);
        }

        public async Task<Result<AccommodationProposalOwnerResponse>> GetAccommodarionProposalByIdOwnerAsync(int id, int ownerId)
        {
            var proposal = await _dbContext.AccommodationProposal
                .Include(p => p.Accommodation)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (proposal == null)
            {
                return new Result<AccommodationProposalOwnerResponse>(false, null, "Proposal not found");
            }

            if (proposal.Accommodation.OwnerId != ownerId)
            {
                return new Result<AccommodationProposalOwnerResponse>(false, null, "You do not have permission to view this proposal");
            }

            var response = _mapper.Map<AccommodationProposalOwnerResponse>(proposal);
            return new Result<AccommodationProposalOwnerResponse>(true, response, null);
        }

        public async Task<Result<bool>> DeleteAccommodationProposalAsync(int id, int ownerId)
        {
            var proposal = await _dbContext.AccommodationProposal
                .Include(p => p.Accommodation)
                .Include(p => p.Bookings)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
            {
                return new Result<bool>(false, false, "Proposal not found");
            }

            if (proposal.Accommodation.OwnerId != ownerId)
            {
                return new Result<bool>(false, false, "You do not have permission to delete this proposal");
            }

            if (proposal.IsActive)
            {
                return new Result<bool>(false, false, "Cannot delete an active proposal");
            }


            bool hasFutureBookings = proposal.Bookings.Any(b => b.EndDate >= DateTime.UtcNow);

            if (hasFutureBookings)
            {
                return new Result<bool>(false, false, "Cannot delete proposal with active or upcoming bookings");
            }

            _dbContext.AccommodationProposal.Remove(proposal);
            await _dbContext.SaveChangesAsync();
            return new Result<bool>(true, true, null);
        }

        public async Task<Result<AccommodationProposalOwnerResponse>> UpdateAccommodationProposalAsync(
            int id,
            AccommodationProposalUpdateRequest request,
            int ownerId)
        {
            var proposal = await _dbContext.AccommodationProposal
                .Include(p => p.Accommodation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
                return new Result<AccommodationProposalOwnerResponse>(false, null, "Proposal not found");

            if (proposal.Accommodation.OwnerId != ownerId)
                return new Result<AccommodationProposalOwnerResponse>(false, null, "You do not have permission to update this proposal");

            if (request.Name != null)
                proposal.Name = request.Name;

            if (request.GuestCount.HasValue)
                proposal.GuestCount = request.GuestCount.Value;

            if (request.PricePerNight.HasValue)
                proposal.PricePerNight = request.PricePerNight.Value;

            if (request.Quantity.HasValue)
                proposal.Quantity = request.Quantity.Value;

            if (request.IsActive.HasValue)
                proposal.IsActive = request.IsActive.Value;

            proposal.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<AccommodationProposalOwnerResponse>(proposal);
            return new Result<AccommodationProposalOwnerResponse>(true, response, null);
        }

    }
}
