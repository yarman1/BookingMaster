using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class PropertyType
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    }
}
