using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.AppLayer.Interfaces
{
    public interface IEmailConfirmRepository
    {
        public Task GenerateManualConfirmation(Guid userID);

        public Task<email_verifications?> GetSavedToken(Guid userID);

        public Task<UserTokenVerificationDto?> GetSavedTokenByEmail(string confirmToken, string email);


        public Task RemoveTokenAfterConfirmation(string Token);





    }
}
