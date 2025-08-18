using BookingMaster.Models;

namespace BookingMaster.Dtos.Accommodation
{
    public class AccommodationSearchResponse
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string City { get; set; }

        public double? AverageRating { get; set; }

        public required PropertyTypeAccommodationResponse PropertyType { get; set; }
    }
}
