using BookingMaster.Models;

namespace BookingMaster.Data
{
    public class DBSeeder
    {
        public static void Seed(ApplicationDBContext context)
        {
            // Seed Roles
            var roles = new List<Role>
            {
                new() { Name = "Owner", NormalizedName = "OWNER" },
                new() { Name = "Customer", NormalizedName = "CUSTOMER" },
            };
            
            foreach (var role in roles)
            {
                if (!context.Role.Any(r => r.Name == role.Name))
                {
                    context.Role.Add(role);
                }
            }

            // Seed BookingStatuses
            var bookingStatuses = new List<BookingStatus>
            {
                new() { Name = "Pending", NormalizedName = "PENDING" },
                new() { Name = "Confirmed", NormalizedName = "CONFIRMED" },
                new() { Name = "Cancelled", NormalizedName = "CANCELLED" },
            };

            foreach (var status in bookingStatuses)
            {
                if (!context.BookingStatus.Any(bs => bs.Name == status.Name))
                {
                    context.BookingStatus.Add(status);
                }
            }

            // Seed PropertyTypes
            var propertyTypes = new List<PropertyType>
            {
                new() { Name = "Apartment" },
                new() { Name = "House" },
                new() { Name = "Hotel" },
                new() { Name = "Hostel" }
            };

            foreach (var type in propertyTypes)
            {
                if (!context.PropertyType.Any(pt => pt.Name == type.Name))
                {
                    context.PropertyType.Add(type);
                }
            }

            context.SaveChanges();
        }
    }
}
