using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Domains.interfaces
{
    public interface IPasswordHasher
    {
       public string Hash(string password);
       public bool Verfiy(string password, string passwordHash);
    }
}
