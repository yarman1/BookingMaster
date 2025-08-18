namespace BookingMaster.Dtos.Bookings
{
    public class AccommodationProposalBookingResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GuestCount { get; set; }
    }
}
