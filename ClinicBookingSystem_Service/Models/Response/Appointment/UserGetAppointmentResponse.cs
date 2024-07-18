using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.Models.Response.AppointmentService;
using ClinicBookingSystem_Service.Models.Response.Meeting;
using ClinicBookingSystem_Service.Models.Response.Result;

namespace ClinicBookingSystem_Service.Models.Response.Appointment;

public class UserGetAppointmentResponse
{
    public int Id { get; set; }
    public DateOnly? Date { get; set; }
    public bool? IsReExam { get; set; }
    public AppointmentStatus? Status { get; set; }
    public string? Description { get; set; }
    public string? FeedBack { get; set; }
    public int? UserAccountId { get; set; }
    public string? UserAccountName { get; set; }
    public int? PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? PatientGender { get; set; }
    public string? PatientPhoneNumber { get; set; }
    public string? PatientAddress { get; set; }
    public string? PatientDateOfBirth { get; set; }
    public string? PatientCCCD { get; set; }
    public string? PatientType { get; set; }
    public bool? IsClinicalExamPaid { get; set; }
    public bool? IsFullyPaid { get; set; }
    public int? SlotId { get; set; }
    public string? SlotName { get; set; }
    public long? TotalUnPaid { get; set; }
    public TimeSpan? StartAt { get; set; }
    public TimeSpan? EndAt { get; set; }
    public GetResultResponse? Result { get; set; }
    public ICollection<GetAppointmentServiceResponse>? AppointmentServices { get; set; }
}