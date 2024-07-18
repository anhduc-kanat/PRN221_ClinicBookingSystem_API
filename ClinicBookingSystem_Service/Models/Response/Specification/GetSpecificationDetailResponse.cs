using System.Runtime.Serialization;
using ClinicBookingSystem_Service.Models.Response.Service;
using Newtonsoft.Json;

namespace ClinicBookingSystem_Service.Models.Response.Specification;

public class GetSpecificationDetailResponse : GetSpecificationResponse
{
    public ICollection<GetServiceResponse> BusinessService { get; set; }
}