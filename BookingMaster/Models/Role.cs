using System.ComponentModel.DataAnnotations.Schema;

namespace BookingMaster.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string Name { get; set; } = string.Empty;
        
        public string NormalizedName { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
