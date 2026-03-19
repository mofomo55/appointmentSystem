using AppointmentBooking.Domains.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Persistencee.Repositories
{
    public class PasswordHasherService : IPasswordHasher
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

    }
}
