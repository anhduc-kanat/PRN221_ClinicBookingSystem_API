using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_BusinessObject.Entities
{
    public class UserProfile:BaseEntities
    {
        //User
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CCCD { get; set; }
        public string? Email { get; set; }
        public int? GroupId { get; set; }
        public string? Type { get; set; }
        public User? User { get; set; }

        //Medical Record
        public ICollection<Result>? Results { get; set; }

     
    }
}