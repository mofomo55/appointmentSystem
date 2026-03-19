using AppointmentBooking.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.AppLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User?> GetUseById(int id);

        Task<User?> GetByEmailAsync(string email);

        Task<User> AddNewUser(User user);

         Task<User?> UpdateOneUser(int id, User updatedUser);

        Task<bool> DeleteOneUser(int id);
    }
}
