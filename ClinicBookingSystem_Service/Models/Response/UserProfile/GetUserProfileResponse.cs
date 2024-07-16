using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.UserProfile
{
    public class GetUserProfileResponse
    {
        public int Id { get; set; }
        //User
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string CCCD { get; set; }
        public string Email { get; set; }
        public int GroupId { get; set; }
        public string Type { get; set; }
    }
}