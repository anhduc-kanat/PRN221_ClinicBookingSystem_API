using ClinicBookingSystem_BusinessObject.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem_BusinessObject.Entities;

public class BusinessService : BaseEntities
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ExpectedDurationInMinute { get; set; }
    public bool? IsPreBooking { get; set; }
    public ServiceType ServiceType { get; set; }

    public long Price { get; set; }
    //Appointment
    public ICollection<AppointmentBusinessService>? AppointmentBusinessServices { get; set; }
    //User
    public ICollection<User>? Users { get; set; }

    //Specification
    public Specification? Specification { get; set; }
}