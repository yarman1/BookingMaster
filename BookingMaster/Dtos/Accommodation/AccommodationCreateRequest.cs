using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Accommodation
{
    public class AccommodationCreateRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 4,
            ErrorMessage = "Name must be between 4 and 30 characters")]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required]
        [RegularExpression(@"^\+380\d{9}$",
            ErrorMessage = "Phone must be in format +380XXXXXXXXX")]
        public required string Phone { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2,
            ErrorMessage = "City must be between 2 and 25 characters")]
        public required string City { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 10,
            ErrorMessage = "Description must be between 10 and 200 characters")]
        public required string Description { get; set; }

        [Required]
        public required TimeOnly CheckInTime { get; set; }

        [Required]
        public required TimeOnly CheckOutTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "PropertyTypeId must be a valid ID")]
        public required int PropertyTypeId { get; set; }
    }
}
