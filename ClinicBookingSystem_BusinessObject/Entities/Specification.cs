using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem_BusinessObject.Entities;

public class Specification : BaseEntities
{
    public string Name { get; set; }
    public string? Description { get; set; }
    
    //User
    public ICollection<BusinessService>? BusinessServices { get; set; }
    public ICollection<User>? User { get; set; }
    
}