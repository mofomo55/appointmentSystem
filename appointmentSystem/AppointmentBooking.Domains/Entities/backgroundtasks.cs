using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.Domains.Entities
{
    public class backgroundtasks
    {
        public int Id { get; set; }
        public string TaskType { get; set; }

        public string Data { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}
