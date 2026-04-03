using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.AppLayer.DTO
{
    public class UserTokenVerificationDto
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
