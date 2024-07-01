﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Models.Response.Authen
{
    public class LoginResponse
    {
        public string AccessToken {  get; set; }
        public DateTime Expired { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public DateTime ExpiredRefreshToken { get; set; }
    }
}
