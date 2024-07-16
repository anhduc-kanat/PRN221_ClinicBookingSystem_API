using ClinicBookingSystem_Service.Models.Response.Service;

namespace ClinicBookingSystem_Service.Models.Response.Order;

public class GetOrderResponse
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Code { get; set; }
    public string name { get; set; }
    public string Description { get; set; }
    public ICollection<GetServiceResponse> Services { get; set; }
}