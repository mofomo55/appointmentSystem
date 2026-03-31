using AppointmentBooking.Domains.interfaces;
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

    }
}
