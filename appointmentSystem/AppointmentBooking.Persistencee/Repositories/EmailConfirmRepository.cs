using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Persistencee.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Persistencee.Repositories
{
    public class EmailConfirmRepository : IEmailConfirmRepository
    {

        private readonly AppDbContext _context;

        public EmailConfirmRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task GenerateManualConfirmation(Guid userID)
        {
           var token = Guid.NewGuid().ToString();

           email_verifications emailTokenForUser = new email_verifications(userID, token, DateTime.UtcNow.AddHours(24));

           await _context.email_verifications.AddAsync(emailTokenForUser);

           await _context.SaveChangesAsync();
        }

        public async Task<email_verifications?> GetSavedToken(Guid userID)
        {
            return await _context.email_verifications
            .FirstOrDefaultAsync(p => p.user_id == userID && p.expires_at > DateTime.UtcNow);
        }

        public async Task<UserTokenVerificationDto?> GetSavedTokenByEmail(string confirmToken, string email)
        {
            return await (from pers in _context.users
                          join emailConfirm in _context.email_verifications
                          on pers.Id equals emailConfirm.user_id
                          where pers.Email == email && emailConfirm.token == confirmToken
                          select new UserTokenVerificationDto
                          {
                              Email = pers.Email,
                              Token = emailConfirm.token,
                              ExpiresAt = emailConfirm.expires_at
                          })
                          .FirstOrDefaultAsync();
        }

        public async Task RemoveTokenAfterConfirmation(string Token)
        {
            var tToken_Email = await _context.email_verifications.FirstOrDefaultAsync(u => u.token == Token);
            _context.email_verifications.Remove(tToken_Email);
            await _context.SaveChangesAsync();

        }

    }
}
