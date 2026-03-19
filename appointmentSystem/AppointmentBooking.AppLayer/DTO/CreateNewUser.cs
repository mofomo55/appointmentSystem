using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppointmentBooking.Domains.Enums;

namespace AppointmentBooking.AppLayer.DTO
{
    public class CreateNewUserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string password { get; set; }

        public UserRole? Role { get; set; }

        public string Phone { get; set; }

    }

}
