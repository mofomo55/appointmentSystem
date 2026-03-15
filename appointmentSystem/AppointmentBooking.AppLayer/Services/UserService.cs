using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.AppLayer.Services
{
    public class UserService
    {
        //Business logic here in this file
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository Userrepository)
        {
            _userRepository = Userrepository;
        }


        public async Task<List<User>> GetUsers()
        {
            var allUsers = await _userRepository.GetAllUsers();
            //var filteredUsers = allUsers
            // .Where(u => u.Role == "user")
            // .ToList();
            return allUsers;
        }

        public async Task<User> GetOneUser(int id)
        {
            var selectedUsers = await _userRepository.GetUseById(id);
            if (id <= 0)
                return null;
            return selectedUsers;
        }

        public async Task<User?> AddUser(User user)
        {
            if (user == null)
                return null;

            var newUser = await _userRepository.AddNewUser(user);

            return newUser;
        }

        public async Task<User?> UpdateUser(int id, User user)
        {
            return await _userRepository.UpdateOneUser(id, user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteOneUser(id);
        }
    }
}
