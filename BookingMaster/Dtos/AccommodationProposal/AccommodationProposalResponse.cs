namespace BookingMaster.Dtos.AccommodationProposal
{
    public class AccommodationProposalResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GuestCount { get; set; }

        public decimal PricePerNight { get; set; }

        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }
    }
}
