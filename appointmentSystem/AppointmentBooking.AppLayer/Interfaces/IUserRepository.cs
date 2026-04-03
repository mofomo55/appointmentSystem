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

        Task<User?> GetUseById(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<User> AddNewUser(User user);

         Task<User?> UpdateOneUser(Guid id, User updatedUser);

        Task<bool> DeleteOneUser(Guid id);

        Task<User?> GetByRefreshTokenAsync(string refreshToken);

        Task SetConfirmationStatus(string email,bool status);


    }
}
