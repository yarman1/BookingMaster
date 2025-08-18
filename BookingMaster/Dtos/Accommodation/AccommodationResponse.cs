using BookingMaster.Models;

namespace BookingMaster.Dtos.Accommodation
{
    public class AccommodationResponse
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;
        
        public double? AverageRating { get; set; }
        
        public string Description { get; set; } = string.Empty;

        public TimeOnly CheckInTime { get; set; }

        public TimeOnly CheckOutTime { get; set; }

        public required PropertyTypeAccommodationResponse PropertyType { get; set; }
    }
}
