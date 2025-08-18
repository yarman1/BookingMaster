using BookingMaster.Dtos.Accommodation;

namespace BookingMaster.Interfaces
{
    public interface IAccommodationService
    {
        Task<Result<AccommodationResponse>> CreateAccommodationAsync(AccommodationCreateRequest request, int ownerId);
        Task<Result<AccommodationResponse>> UpdateAccommodationAsync(int id, AccommodationUpdateRequest request, int ownerId);
        Task<Result<AccommodationResponse>> GetAccommodationByIdAsync(int id);
        Task<Result<IEnumerable<AccommodationSearchResponse>>> SearchAccommodationsAsync(AccommodationQuery query);
        Task<Result<IEnumerable<AccommodationSearchResponse>>> GetOwnerAccomodationsAsync(int ownerId);
        Task<Result<bool>> DeleteAccommodationAsync(int id, int ownerId);
    }
}
