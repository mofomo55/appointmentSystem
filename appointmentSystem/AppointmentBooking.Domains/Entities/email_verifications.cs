using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Domains.Entities
{
    public class email_verifications
    {
        public int Id { get; set; }
        public Guid? user_id { get; set; }
        public string? token { get; set; }

        public DateTime? expires_at { get; set; }

        public DateTime Created_at { get; set; }

        private email_verifications() { }

        public email_verifications(Guid user_id, string token, DateTime expires_at )
        {
            this.user_id = user_id;
            this.token = token;
            this.expires_at = expires_at;
        }
    }
}
