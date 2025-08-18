using AutoMapper;
using BookingMaster.Data;
using BookingMaster.Dtos.Accommodation;
using BookingMaster.Interfaces;
using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingMaster.Services
{
    public class AccommodationService(ApplicationDBContext dbContext, IMapper mapper) : IAccommodationService
    {
        private readonly ApplicationDBContext _dbContext = dbContext;

        private readonly IMapper _mapper = mapper;

        public async Task<Result<AccommodationResponse>> CreateAccommodationAsync(AccommodationCreateRequest request, int ownerId)
        {
            if (_dbContext.Accommodation.Any(acc => acc.Email == request.Email))
            {
                return new Result<AccommodationResponse>(false, null, "Accommodation with this email already exists");
            }

            if (_dbContext.Accommodation.Any(acc => acc.Phone == request.Phone))
            {
                return new Result<AccommodationResponse>(false, null, "Accommodation with this phone number already exists");
            }

            User? owner = await _dbContext.User.FindAsync(ownerId);

            if (owner == null)
            {
                return new Result<AccommodationResponse>(false, null, "Owner not found");
            }

            PropertyType? propertyType = await _dbContext.PropertyType.FindAsync(request.PropertyTypeId);

            if (propertyType == null)
            {
                return new Result<AccommodationResponse>(false, null, "Property type not found");
            }

            Accommodation accommodation = new()
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                City = request.City,
                Description = request.Description,
                CheckInTime = request.CheckInTime,
                CheckOutTime = request.CheckOutTime,
                PropertyTypeId = request.PropertyTypeId,
                OwnerId = ownerId,
                AverageRating = null,
                Owner = owner,
                PropertyType = propertyType
            };

            await _dbContext.Accommodation.AddAsync(accommodation);
            await _dbContext.SaveChangesAsync();

            AccommodationResponse accommodationResponse = _mapper.Map<AccommodationResponse>(accommodation);

            return new Result<AccommodationResponse>(
                true,
                accommodationResponse,
                null
            );
        }

        public async Task<Result<AccommodationResponse>> UpdateAccommodationAsync(
            int id,
            AccommodationUpdateRequest request,
            int ownerId)
        {
            var accommodation = await _dbContext.Accommodation
                .Include(a => a.PropertyType)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (accommodation == null)
                return new Result<AccommodationResponse>(false, null, "Accommodation not found");

            if (accommodation.OwnerId != ownerId)
                return new Result<AccommodationResponse>(false, null, "You are not authorized to update this accommodation");

            if (!string.IsNullOrWhiteSpace(request.Name))
                accommodation.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.Email))
                accommodation.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Phone))
                accommodation.Phone = request.Phone;

            if (!string.IsNullOrWhiteSpace(request.City))
                accommodation.City = request.City;

            if (!string.IsNullOrWhiteSpace(request.Description))
                accommodation.Description = request.Description;

            if (request.CheckInTime.HasValue)
                accommodation.CheckInTime = request.CheckInTime.Value;

            if (request.CheckOutTime.HasValue)
                accommodation.CheckOutTime = request.CheckOutTime.Value;

            if (request.PropertyTypeId.HasValue)
            {
                var propertyType = await _dbContext.PropertyType.FindAsync(request.PropertyTypeId.Value);
                if (propertyType == null)
                    return new Result<AccommodationResponse>(false, null, "Property type not found");

                accommodation.PropertyTypeId = propertyType.Id;
                accommodation.PropertyType = propertyType;
            }

            accommodation.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            var response = _mapper.Map<AccommodationResponse>(accommodation);
            return new Result<AccommodationResponse>(true, response, null);
        }

        public async Task<Result<AccommodationResponse>> GetAccommodationByIdAsync(int id)
        {
            var accommodation = await _dbContext.Accommodation
                .Include(acc => acc.PropertyType)
                .FirstOrDefaultAsync(acc => acc.Id == id);
            if (accommodation == null)
            {
                return new Result<AccommodationResponse>(false, null, "Accommodation not found");
            }
            AccommodationResponse accommodationResponse = _mapper.Map<AccommodationResponse>(accommodation);
            return new Result<AccommodationResponse>(
                true,
                accommodationResponse,
                null
            );
        }

        public async Task<Result<IEnumerable<AccommodationSearchResponse>>> SearchAccommodationsAsync(AccommodationQuery query)
        {
            IQueryable<Accommodation> accommodations = _dbContext.Accommodation
                .Include(acc => acc.PropertyType);

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                accommodations = accommodations.Where(acc => acc.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.City))
            {
                accommodations = accommodations.Where(acc => acc.City.Equals(query.City));
            }

            if (query.PropertyTypeId.HasValue)
            {
                accommodations = accommodations.Where(acc => acc.PropertyTypeId == query.PropertyTypeId.Value);
            }

            if (query.SortBy?.Equals("AverageRating", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (query.SortDescending)
                {
                    accommodations = accommodations
                        .OrderByDescending(acc => acc.AverageRating.HasValue)
                        .ThenByDescending(acc => acc.AverageRating);
                }
                else
                {
                    accommodations = accommodations
                        .OrderByDescending(acc => acc.AverageRating.HasValue)
                        .ThenBy(acc => acc.AverageRating);
                }
            }
            else
            {
                accommodations = accommodations.OrderBy(acc => acc.Id);
            }

            var paged = await accommodations
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var results = _mapper.Map<IEnumerable<AccommodationSearchResponse>>(paged);

            return new Result<IEnumerable<AccommodationSearchResponse>>(true, results, null);
        }

        public async Task<Result<IEnumerable<AccommodationSearchResponse>>> GetOwnerAccomodationsAsync(int ownerId)
        {
            var accommodations = await _dbContext.Accommodation
                .Where(acc => acc.OwnerId == ownerId)
                .Include(acc => acc.PropertyType)
                .ToListAsync();
            if (accommodations == null || accommodations.Count == 0)
            {
                return new Result<IEnumerable<AccommodationSearchResponse>>(false, null, "No accommodations found for this owner");
            }
            var results = _mapper.Map<IEnumerable<AccommodationSearchResponse>>(accommodations);
            return new Result<IEnumerable<AccommodationSearchResponse>>(true, results, null);
        }

        public async Task<Result<bool>> DeleteAccommodationAsync(int id, int ownerId)
        {
            var accommodation = await _dbContext.Accommodation.FindAsync(id);
            if (accommodation == null)
            {
                return new Result<bool>(false, false, "Accommodation not found");
            }
            if (accommodation.OwnerId != ownerId)
            {
                return new Result<bool>(false, false, "You are not authorized to delete this accommodation");
            }
            _dbContext.Accommodation.Remove(accommodation);
            await _dbContext.SaveChangesAsync();
            return new Result<bool>(true, true, null);
        }
    }
}
