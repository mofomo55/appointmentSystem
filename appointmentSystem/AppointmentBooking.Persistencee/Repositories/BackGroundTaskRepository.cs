using AppointmentBooking.AppLayer.DTO;
using AppointmentBooking.AppLayer.Interfaces;
using AppointmentBooking.Domains.Entities;
using AppointmentBooking.Persistencee.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppointmentBooking.Persistencee.Repositories
{
    public class BackGroundTaskRepository: IBackgroundTaskGeneration

    {
        private readonly AppDbContext _context;

        public BackGroundTaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateNewEmailTask(BackgroundTaskDataEmail data)
        {
            try
            {
                XElement xmlData = new XElement("Email",
                    new XElement("To", data.To),
                    new XElement("Subject", data.Subject),
                    new XElement("Body", data.Body)
                );

                var newTask = new backgroundtasks
                {
                    // Do NOT set Id here at all
                    TaskType = "Email",
                    Data = xmlData.ToString(),
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                _context.backgroundtasks.Add(newTask);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // This will tell you EXACTLY why the second row isn't adding
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
        }

        //public async Task CreateNewProcdurelTask(BackgroundTaskDataEmail data)
        //{

        //    XElement xmlData = new XElement("Email",
        //        new XElement("To", data.To),
        //        new XElement("Subject", data.Subject),
        //        new XElement("Body", data.Body)
        //    );

        //    string xmlString = xmlData.ToString();

        //    var newTask = new backgroundtasks
        //    {
        //        TaskType = "Email",
        //        Data = xmlString,
        //        Status = "Pending",
        //        CreatedAt = DateTime.UtcNow // Assuming you have other columns
        //    };

        //    _context.backgroundtasks.Add(newTask);
        //    await _context.SaveChangesAsync();
        //}

    }
}
