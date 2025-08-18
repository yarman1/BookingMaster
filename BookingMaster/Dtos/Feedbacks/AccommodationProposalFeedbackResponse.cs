namespace BookingMaster.Dtos.Feedbacks
{
    public class AccommodationProposalFeedbackResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GuestCount { get; set; }

        public decimal PricePerNight { get; set; }
    }
}
