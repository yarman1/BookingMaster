using System.ComponentModel.DataAnnotations;

namespace BookingMaster.Dtos.Bookings
{
    public class BookingQuery
    {
        [StringLength(15, ErrorMessage = "Status must be valid")]
        public string? Status { get; set; }
    }
}
