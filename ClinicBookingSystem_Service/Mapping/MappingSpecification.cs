using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Request.Specification;
using ClinicBookingSystem_Service.Models.Response.Specification;

namespace ClinicBookingSystem_Service.Mapping;

public class MappingSpecification : Profile
{
    public MappingSpecification()
    {
        CreateMap<CreateSpecificationRequest, Specification>().ReverseMap();
        CreateMap<Specification, CreateSpecificationRequest>().ReverseMap();
        CreateMap<UpdateSpecificationRequest, Specification>().ReverseMap();
        CreateMap<Specification, UpdateSpecificationRequest>();
        
        CreateMap<Specification, GetSpecificationResponse>().ReverseMap();
        CreateMap<Specification, CreateSpecificationResponse>().ReverseMap();
        CreateMap<GetSpecificationResponse, GetSpecificationDetailResponse>().ReverseMap();
        CreateMap<Specification, GetSpecificationDetailResponse>().ReverseMap();
    }
}