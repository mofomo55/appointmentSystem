using AppointmentBooking.AppLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentBooking.AppLayer.Interfaces
{
    public interface IBackgroundTaskGeneration
    {
        public Task CreateNewEmailTask(BackgroundTaskDataEmail data);

      //  public Task CreateNewProcdurelTask(BackgroundTaskDataProcdure data);
    }
}
