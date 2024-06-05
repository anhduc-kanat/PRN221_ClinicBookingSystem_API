﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.Dentist
{
    public class GetAllDentistsResponse
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
