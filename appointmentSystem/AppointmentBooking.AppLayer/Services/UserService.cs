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
        private readonly IJwtTokenGenerator _JwtTokenGenerator;

        public UserService(IUserRepository Userrepository, IPasswordHasher Passwordhasher, IJwtTokenGenerator JwtTokenGenerator)
        {
            _userRepository = Userrepository;
            _Passwordhasher = Passwordhasher;
            _JwtTokenGenerator = JwtTokenGenerator;

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

        public async Task<AuthResponseDto?> LoginAsync(LoginDTO dto)
        {
            var tUser = await _userRepository.GetByEmailAsync(dto.Email);

            if (tUser == null)
                throw new Exception("User not found");

            // 👇 المقارنة باستخدام Hasher
            var isValid = _Passwordhasher.Verfiy(dto.password, tUser.Password);

            if (!isValid)
                throw new Exception("Invalid password");

            var accessToken = _JwtTokenGenerator.GenerateToken(tUser);

            // 👇 إنشاء Refresh Token
            var refreshToken = Guid.NewGuid().ToString();

            tUser.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

            await _userRepository.UpdateOneUser(tUser.Id, tUser);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            //   var token = _JwtTokenGenerator.GenerateToken(tUser);

            //   return token;

            //return new UserDTO
            //{
            //    Name = tUser.Name,
            //    Email = tUser.Email,
            //    Role = (UserRole)tUser.Role
            //};
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                throw new Exception("Invalid refresh token");

            var newAccessToken = _JwtTokenGenerator.GenerateToken(user);

            // 🔥 الأفضل: تغيير refresh token كل مرة (Rotation)
            var newRefreshToken = Guid.NewGuid().ToString();

            user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));

            await _userRepository.UpdateOneUser(user.Id, user);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
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
