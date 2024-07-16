namespace ClinicBookingSystem_Service.Models.Request.Order;

public class UpdateOrderRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public IEnumerable<int>? ServiceId { get; set; }
}