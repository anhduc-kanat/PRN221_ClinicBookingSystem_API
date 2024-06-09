﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.Models.Response.Salary;
using ClinicBookingSystem_Service.Models.Response.Specification;

namespace ClinicBookingSystem_Service.Models.Response.Dentist
{
    public class GetDentistByIdResponse
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<GetSpecificationResponse> Specifications { get; set; }
        public long SalaryAmount { get; set; }
    }
}
