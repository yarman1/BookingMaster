using BookingMaster.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingMaster.Data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> User { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<BookingStatus> BookingStatus { get; set; }

        public DbSet<AccommodationProposal> AccommodationProposal { get; set; }

        public DbSet<Accommodation> Accommodation { get; set; }

        public DbSet<PropertyType> PropertyType { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }
    }
}
