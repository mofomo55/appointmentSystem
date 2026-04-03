using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Domains.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppointmentBooking.Domains.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentBooking.AppLayer.Services
{
    public class UserService
    {
        //Business logic here in this file
        private readonly IEmailConfirmRepository _EmailConfirmRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManagment _Passwordhasher;
        private readonly IJwtTokenGenerator _JwtTokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository Userrepository, IPasswordManagment Passwordhasher, IJwtTokenGenerator JwtTokenGenerator, IEmailConfirmRepository EmailConfirmRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = Userrepository;
            _Passwordhasher = Passwordhasher;
            _JwtTokenGenerator = JwtTokenGenerator;
            _EmailConfirmRepository = EmailConfirmRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<List<User>> GetUsers()
        {
            var allUsers = await _userRepository.GetAllUsers();
            return allUsers;
        }

        public async Task<User> GetOneUser(Guid id)
        {
            var selectedUsers = await _userRepository.GetUseById(id);
            if (selectedUsers == null)
                return null;
            return selectedUsers;
        }

        public async Task<UserDTO?> AddUser(CreateNewUserDTO dto)
        {


            var tEnterdEmail = await _userRepository.GetByEmailAsync(dto.Email);

            if (tEnterdEmail != null)
            {
                throw new Exception("this User is Already Registered");
            }

            var tPasswordValidation = _Passwordhasher.Validate(dto.password);

            if (tPasswordValidation.Any())
            {
                throw new ArgumentException(string.Join(" | ", tPasswordValidation));
            }

            var hashedpassword = _Passwordhasher.Hash(dto.password);

            var tUser = new User(dto.Name, dto.Email, hashedpassword, dto.Phone, dto.Role);

            await _userRepository.AddNewUser(tUser);

            if (await _EmailConfirmRepository.GetSavedToken(tUser.Id) == null)
            {
                await _EmailConfirmRepository.GenerateManualConfirmation(tUser.Id);
            }


            var tSavedEmailData = await _EmailConfirmRepository.GetSavedToken(tUser.Id);

            var tSavedToken = tSavedEmailData.token;

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            // 4. بناء الرابط
            var confirmationLink = $"{baseUrl}/api/Users/confirm-email?token={tSavedToken}&email={tUser.Email}";

            return new UserDTO
            {
              Id = tUser.Id,
              Name = tUser.Name,
              Email = tUser.Email,
              Role = (UserRole)tUser.Role,
              emailconfirmLink = confirmationLink
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

        public async Task<bool?> ConfirmEmail(string token, string email)
        {
           var tUserToken =  await _EmailConfirmRepository.GetSavedTokenByEmail(token, email);

            if (tUserToken == null || tUserToken.ExpiresAt < DateTime.UtcNow)
            {
                throw new Exception("Invalid Link or expired!!"); 
            }

            await _userRepository.SetConfirmationStatus(email,true);

            await _EmailConfirmRepository.RemoveTokenAfterConfirmation(token);

            return true;
        }



        public async Task<User?> UpdateUser(Guid id, User user)
        {
            return await _userRepository.UpdateOneUser(id, user);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            return await _userRepository.DeleteOneUser(id);
        }
    }
}
