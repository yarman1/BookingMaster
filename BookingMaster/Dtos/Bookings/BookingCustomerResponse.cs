namespace BookingMaster.Dtos.Bookings
{
    public class BookingCustomerResponse
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public string? Comment { get; set; }

        public required string Status { get; set; }

        public FeedbackBookingResponse? Feedback { get; set; }

        public required AccommodationBookingResponse Accommodation { get; set; }

        public required AccommodationProposalBookingResponse AccommodationProposal { get; set; }
    }
}
