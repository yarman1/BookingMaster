using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Accommodation
{
    public class AccommodationUpdateRequest
    {
        [StringLength(30, MinimumLength = 4,
    ErrorMessage = "Name must be between 4 and 30 characters")]
        public string? Name { get; set; }

  
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [RegularExpression(@"^\+380\d{9}$",
            ErrorMessage = "Phone must be in format +380XXXXXXXXX")]
        public string? Phone { get; set; }

        [StringLength(25, MinimumLength = 2,
            ErrorMessage = "City must be between 2 and 25 characters")]
        public string? City { get; set; }

        [StringLength(200, MinimumLength = 10,
            ErrorMessage = "Description must be between 10 and 200 characters")]
        public string? Description { get; set; }

        public TimeOnly? CheckInTime { get; set; }

        public TimeOnly? CheckOutTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PropertyTypeId must be a valid ID")]
        public int? PropertyTypeId { get; set; }
    }
}
