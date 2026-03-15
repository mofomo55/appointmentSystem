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

    }
}