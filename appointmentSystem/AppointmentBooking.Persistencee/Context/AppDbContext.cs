using Microsoft.EntityFrameworkCore;
using AppointmentBooking.Domains.Entities;

namespace AppointmentBooking.Persistencee.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Created_at)
                .HasDefaultValueSql("GETDATE()");
        }

    }
}