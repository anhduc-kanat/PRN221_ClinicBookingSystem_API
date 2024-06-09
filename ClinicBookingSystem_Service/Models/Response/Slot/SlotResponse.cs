﻿using ClinicBookingSystem_BusinessObject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.Models.Response.Appointment;

namespace ClinicBookingSystem_Service.Models.Response.Slot
{
    public class SlotResponse
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public TimeSpan? StartAt { get; set; }
        public TimeSpan? EndAt { get; set; }
        public SlotStatus? Status { get; set; }
    }
}
