using Microsoft.EntityFrameworkCore;
using AppointmentBooking.Domains.Entities;

namespace AppointmentBooking.Persistencee.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<email_verifications> email_verifications { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map to exact MySQL table names to avoid "Table doesn't exist" errors
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<email_verifications>().ToTable("email_verifications");

            // Role Enum to String
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Default Dates (MySQL Syntax: CURRENT_TIMESTAMP)
            modelBuilder.Entity<User>()
                .Property(u => u.Created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<email_verifications>()
                .Property(u => u.Created_at)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // If email_verifications doesn't have an "Id" column, 
            // you MUST define a Key, for example:
            //modelBuilder.Entity<email_verifications>()
            //    .HasKey(e => e.user_id);
        }

    }
}