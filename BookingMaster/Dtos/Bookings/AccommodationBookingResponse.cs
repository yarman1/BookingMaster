namespace BookingMaster.Dtos.Bookings
{
    public class AccommodationBookingResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }
    }
}
