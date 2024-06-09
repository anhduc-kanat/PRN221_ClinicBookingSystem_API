using System.ComponentModel.DataAnnotations;
using ClinicBookingSystem_BusinessObject.Enums;

namespace ClinicBookingSystem_BusinessObject.Entities;

public class Appointment : BaseEntities
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool? IsPeriod { get; set; } = false;
    public int? ReExamUnit { get; set; }
    public int? ReExamNumber { get; set; }
    public bool? IsApproved { get; set; } = false;
    public AppointmentStatus Status { get; set; }
    public string? Description { get; set; }
    public string? FeedBack {get; set; }
    public bool? IsTreatment { get; set; } = false;
    //User
    public ICollection<User>? Users { get; set; }
    //Service
    public ICollection<Service>? Services { get; set; }
    //Slot
    public Slot? Slots { get; set; }
}
