using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class BookingStatus
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(15)")]
        public string Name { get; set; } = string.Empty;

        public string NormalizedName { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; } = [];
    }
}
