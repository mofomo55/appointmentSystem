using AppointmentBooking.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Domains.interfaces
{
    public interface IPasswordManagment
    {
       public string Hash(string password);
       public bool Verfiy(string password, string passwordHash);

       public  List<string> Validate(string password);

    }
}
