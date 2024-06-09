using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Response.Role;

namespace ClinicBookingSystem_Service.Mapping;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, GetRoleResponse>().ReverseMap();
        CreateMap<Role, CreateRoleResponse>().ReverseMap();
        CreateMap<Role, UpdateRoleResponse>().ReverseMap();
        CreateMap<Role, DeleteRoleResponse>().ReverseMap();
        
    }
}