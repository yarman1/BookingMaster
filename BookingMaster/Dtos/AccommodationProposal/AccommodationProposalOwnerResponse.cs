namespace BookingMaster.Dtos.AccommodationProposal
{
    public class AccommodationProposalOwnerResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int GuestCount { get; set; }

        public decimal PricePerNight { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }
    }
}
