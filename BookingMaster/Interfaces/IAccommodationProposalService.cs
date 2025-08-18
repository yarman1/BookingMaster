using BookingMaster.Dtos.AccommodationProposal;

namespace BookingMaster.Interfaces
{
    public interface IAccommodationProposalService
    {
        Task<Result<AccommodationProposalResponse>> CreateAccommodationProposalAsync(AccommodationProposalCreateRequest request, int ownerId);

        Task<Result<IEnumerable<AccommodationProposalOwnerResponse>>> GetAccommodationProposalsOwnerAsync(int accommodationId, int ownerId);

        Task<Result<IEnumerable<AccommodationProposalResponse>>> GetAccommodationProposalsCustomerAsync(AccommodationProposalAvailabilityRequest request);

        Task<Result<AccommodationProposalOwnerResponse>> GetAccommodarionProposalByIdOwnerAsync(int id, int ownerId);

        Task<Result<bool>> DeleteAccommodationProposalAsync(int id, int ownerId);

        Task<Result<AccommodationProposalOwnerResponse>> UpdateAccommodationProposalAsync(int id, AccommodationProposalUpdateRequest request, int ownerId);
    }
}
