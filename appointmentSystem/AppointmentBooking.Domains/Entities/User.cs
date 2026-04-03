using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static AppointmentBooking.Domains.Enums;


namespace AppointmentBooking.Domains.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email  { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public UserRole? Role { get; set; }

        public bool email_verified { get; set; }

        public DateTime? email_verified_at { get; set; }

        public DateTime Created_at { get; set; }

        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }



        public User(string name,string email,string password,string phone, UserRole? role)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("name is required");
            }

            if (!email.Contains("@"))
            {
                throw new Exception("invalied email");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("password is required");
            }

            Id = Guid.NewGuid();
            
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            Phone = phone;
        }

        public void SetRefreshToken(string token, DateTime expiry)
        {
            RefreshToken = token;
            RefreshTokenExpiryTime = expiry;
        }


    }
}
