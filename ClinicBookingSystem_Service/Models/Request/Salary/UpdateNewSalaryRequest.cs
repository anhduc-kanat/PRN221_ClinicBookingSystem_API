﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Request.Salary
{
    public class UpdateNewSalaryRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? amount { get; set; }
    }
}
