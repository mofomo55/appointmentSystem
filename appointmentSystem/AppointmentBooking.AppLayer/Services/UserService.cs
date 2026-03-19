using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Domains.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppointmentBooking.Domains.Enums;

namespace AppointmentBooking.AppLayer.Services
{
    public class UserService
    {
        //Business logic here in this file
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _Passwordhasher;

        public UserService(IUserRepository Userrepository, IPasswordHasher Passwordhasher)
        {
            _userRepository = Userrepository;
            _Passwordhasher = Passwordhasher;
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

        public async Task<UserDTO?> AddUser(CreateNewUserDTO dto)
        {
            var hashedpassword = _Passwordhasher.Hash(dto.password);

            var tUser = new User(dto.Name, dto.Email, hashedpassword, dto.Phone, dto.Role);

            await _userRepository.AddNewUser(tUser);

            return new UserDTO
            {
                Name = tUser.Name,
                Email = tUser.Email,
                Role = (UserRole)tUser.Role
            };
        }

        public async Task<UserDTO?> LoginAsync(LoginDTO dto)
        {
            var tUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (tUser == null)
                throw new Exception("User not found");

            // 👇 المقارنة باستخدام Hasher
            var isValid = _Passwordhasher.Verfiy(dto.password, tUser.Password);

            if (!isValid)
                throw new Exception("Invalid password");

            return new UserDTO
            {
                Name = tUser.Name,
                Email = tUser.Email,
                Role = (UserRole)tUser.Role
            };
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
