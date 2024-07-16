using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_BusinessObject.Entities
{
    public class Result : BaseEntities
    {
        public int UserTreatmentId { get; set; }
        public string UserTreatmentName { get; set; }
        public int UserAccountId { get; set; }
        public string UserAccountName { get; set; }
        public string? PreScriptionName { get; set; }
        public string? PreScriptionDescription { get; set; }
        //Appointment
        public int AppointmentId { get; set; }
        //UserProfile
        public UserProfile? UserProfile { get; set; }
        //Medicine
        public ICollection<Medicine>? Medicines { get; set; }
        //Note
        public ICollection<Note>? Notes { get; set; }
    }
}