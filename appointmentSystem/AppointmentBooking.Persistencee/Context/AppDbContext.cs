using Microsoft.EntityFrameworkCore;
using AppointmentBooking.Domains.Entities;

namespace AppointmentBooking.Persistencee.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> users { get; set; }
    }
}