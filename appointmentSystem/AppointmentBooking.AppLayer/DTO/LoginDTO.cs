using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppointmentBooking.Domains.Enums;

namespace AppointmentBooking.AppLayer.DTO
{
    public class LoginDTO
    {
        public string Email { get; set; }

        public string password { get; set; }
    }

}
