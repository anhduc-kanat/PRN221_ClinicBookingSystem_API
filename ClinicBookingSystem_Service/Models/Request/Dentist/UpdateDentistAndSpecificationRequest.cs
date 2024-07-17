namespace ClinicBookingSystem_Service.Models.Request.Dentist;

public class UpdateDentistAndSpecificationRequest
{
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public IEnumerable<int> SpecificationId { get; set; }
}