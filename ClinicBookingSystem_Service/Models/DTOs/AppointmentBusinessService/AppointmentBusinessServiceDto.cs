using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;

namespace ClinicBookingSystem_Service.Models.DTOs.AppointmentBusinessService;

public class AppointmentBusinessServiceDto
{
    public AppointmentBusinessServiceStatus Status { get; set; }
    public int DentistId { get; set; }
    public string DentistName { get; set; }
    public string ServiceName { get; set; }
    public long ServicePrice { get; set; }
    public ServiceType ServiceType { get; set; }
    public int UserTreatmentId { get; set; }
    public string UserTreatmentName { get; set; }
    public int UserAccountId { get; set; }
    public string UserAccountName { get; set; }
    public int TotalMeetingDate { get; set; }
    public int MeetingCount { get; set; }
    public BusinessService BusinessService { get; set; }
    public ClinicBookingSystem_BusinessObject.Entities.Appointment Appointment { get; set; }
}