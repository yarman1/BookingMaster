using BookingMaster.Dtos.Bookings;

namespace BookingMaster.Interfaces
{
    public interface IBookingService
    {
        Task<Result<BookingCustomerResponse>> CreateBookingAsync(BookingCreateRequest request, int customerId);

        Task<Result<BookingCustomerResponse>> GetBookingByIdCustomerAsync(int bookingId, int customerId);

        Task<Result<BookingOwnerResponse>> GetBookingByIdOwnerAsync(int bookingId, int ownerId);

        Task<Result<IEnumerable<BookingCustomerResponse>>> GetBookingsByCustomerAsync(BookingQuery query, int customerId);

        Task<Result<IEnumerable<BookingOwnerResponse>>> GetBookingsByOwnerAsync(BookingQuery query, int ownerId);

        Task<Result<BookingOwnerResponse>> ChangeBookingStatus(int bookingId, int ownerId, string newStatus);
    }
}
