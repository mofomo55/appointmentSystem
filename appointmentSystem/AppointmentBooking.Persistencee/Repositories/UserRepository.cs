using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Persistencee.Context;
using AppointmentBooking.AppLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace AppointmentBooking.Persistencee.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<User?> GetUseById(int id)
        {
            return await _context.users
          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<User> AddNewUser(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.users
          .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.users
          .FirstOrDefaultAsync(u => u.RefreshToken.ToLower() == refreshToken.ToLower());
        }


        public async Task<User?> UpdateOneUser(int id, User updatedUser)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            user.Name = updatedUser.Name;
            user.Role = updatedUser.Role;
            user.Email = updatedUser.Email;

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteOneUser(int id)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return false;

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
