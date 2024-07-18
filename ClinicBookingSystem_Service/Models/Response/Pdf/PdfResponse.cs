using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using ClinicBookingSystem_Service.Models.Response.Note;
using ClinicBookingSystem_Service.Models.Response.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.Pdf
{
    public class PdfResponse
    {
        public string Title { get; set; }   
        public string PatientName { get; set; } 
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }  
        public GetNoteResponse Note { get; set; }
        public GetAppointmentResponse Appointment { get; set; }

        

    }
}
