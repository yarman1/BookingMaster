using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
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

        [Column(TypeName = "float")]
        public double? AverageRating { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "time")]
        public TimeOnly CheckInTime { get; set; }

        [Column(TypeName = "time")]
        public TimeOnly CheckOutTime { get; set; }

        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2(0)")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int FeedbackCount { get; set; } = 0;

        public int TotalRating { get; set; } = 0;

        public int PropertyTypeId { get; set; }

        public required PropertyType PropertyType { get; set; }

        public int OwnerId { get; set; }

        public required User Owner { get; set; }

        public ICollection<AccommodationProposal> AccommodationProposals { get; set; } = [];
    }
}
