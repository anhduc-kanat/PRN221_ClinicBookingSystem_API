using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.AppointmentService
{
    public class GetAppointmentServiceName
    {
        public int Id { get; set; }
        public string ServiceName {  get; set; }
    }
}
