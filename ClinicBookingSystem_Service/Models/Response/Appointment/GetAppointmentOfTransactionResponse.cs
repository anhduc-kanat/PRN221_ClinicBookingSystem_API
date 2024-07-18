using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Response.AppointmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.Appointment
{
    public class GetAppointmentOfTransactionResponse
    {
        public int Id { get; set; }
        public string UserAccountName { get; set; }
        public ICollection <GetAppointmentServiceName> Appointment { get; set; }
    }
}
