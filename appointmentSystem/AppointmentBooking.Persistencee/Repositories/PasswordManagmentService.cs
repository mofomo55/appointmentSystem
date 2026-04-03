using AppointmentBooking.Domains.interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Persistencee.Repositories
{
    public class PasswordManagmentService : IPasswordManagment
    {
        private readonly Microsoft.AspNetCore.Identity.PasswordHasher<string> _hasher = new();
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PasswordManagmentService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string Hash(string password)
        {
            return _hasher.HashPassword("", password);
        }

        public bool Verfiy(string password,string PasswordHash)
        {
            var result = _hasher.VerifyHashedPassword(null, PasswordHash, password);
            return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }

        public  List<string> Validate(string password)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                errors.Add("Password must contain at least one uppercase letter.");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add("Password must contain at least one special character.");

            return errors;
        }

       // public async Task<string> GenerateManualConfirmationLink(User user)
    //    {
            // 1. توليد توكين عشوائي فريد
            //var token = Guid.NewGuid().ToString();

            // 2. حفظ التوكين في قاعدة البيانات مع وقت انتهاء (مثلاً 24 ساعة)
         //   user.EmailConfirmationToken = token;
         //   user.TokenExpiration = DateTime.UtcNow.AddHours(24);

         //   await _userRepository.UpdateAsync(user); // حفظ التغييرات

            // 3. جلب الدومين تلقائياً
       //     var request = _httpContextAccessor.HttpContext.Request;
//            var baseUrl = $"{request.Scheme}://{request.Host}";
//
            // 4. بناء الرابط
      //      var confirmationLink = $"{baseUrl}/api/auth/confirm-email?token={token}&email={user.Email}";

      //      return confirmationLink;
      //  }

    }
}
