namespace BookingMaster.Dtos.Bookings
{
    public class FeedbackBookingResponse
    {
        public int Id { get; set; }

        public string Comment { get; set; } = string.Empty;

        public int Rating { get; set; }
    }
}
