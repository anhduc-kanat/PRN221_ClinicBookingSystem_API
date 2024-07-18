using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.Models.Response.Meeting;

namespace ClinicBookingSystem_Service.Models.Response.AppointmentService;

public class GetAppointmentServiceResponse
{
    public int Id { get; set; }
    public AppointmentBusinessServiceStatus Status { get; set; }
    public int DentistId { get; set; }
    public string DentistName { get; set; }
    public int BusinessServiceId { get; set; }
    public string ServiceName { get; set; }
    public long ServicePrice { get; set; }
    public ServiceType ServiceType { get; set; }
    public bool IsPaid { get; set; }
    public int UserTreatmentId { get; set; }
    public string UserTreatmentName { get; set; }
    public int UserAccountId { get; set; }
    public string UserAccountName { get; set; }
    public int TotalMeetingDate { get; set; }
    public int MeetingCount { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public ICollection<GetMeetingResponse>? Meetings { get; set; }
}