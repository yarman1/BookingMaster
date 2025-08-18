namespace BookingMaster.Dtos.Feedbacks
{
    public class FeedbackResponse
    {
        public int Id { get; set; }

        public string Comment { get; set; } = string.Empty;

        public int Rating { get; set; }

        public required CustomerFeedbackResponse Customer { get; set; }

        public required AccommodationProposalFeedbackResponse AccommodationProposal { get; set; }
    }
}
