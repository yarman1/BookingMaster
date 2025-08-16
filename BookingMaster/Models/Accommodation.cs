using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class Accommodation
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(13)")]
        public string Phone { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(25)")]
        public string City { get; set; } = string.Empty;

        [Column(TypeName = "decimal(3,2)")]
        public double AverageRating { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "time")]
        public TimeOnly CheckInTime { get; set; }

        [Column(TypeName = "time")]
        public TimeOnly CheckOutTime { get; set; }

        public int PropertyTypeId { get; set; }

        public required PropertyType PropertyType { get; set; }

        public int OwnerId { get; set; }

        public required User Owner { get; set; }
    }
}
