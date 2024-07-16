using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Request.Customer;
using ClinicBookingSystem_Service.Models.Request.Relative;
using ClinicBookingSystem_Service.Models.Request.UserProfile;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Response.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Mapping
{
    public class MappingUserProfile:Profile
    {
        public MappingUserProfile()
        {
            CreateMap<CreateUserProfileRequest, UserProfile>().ReverseMap();
            CreateMap<UserProfile, DeleteUserProfileResponse>();
            CreateMap<UpdateUserProfileRequest, UserProfile>().ReverseMap();
            CreateMap<UserProfile, UpdateUserProfileResponse>();
            CreateMap<UserProfile, GetUserProfileResponse>();
            CreateMap<UserProfile, CreateUserProfileResponse>();

            CreateMap<CreateUserProfileRequest, User>().ReverseMap();
        }
    }
}