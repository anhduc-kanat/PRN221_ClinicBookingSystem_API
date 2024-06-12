﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Request.MedicalRecord
{
    public class UpdateMedicalRecordRequest
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public int UserProfileId { get; set; }
    }
}