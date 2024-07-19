namespace ClinicBookingSystem_Service.Models.Request.Appointment;

public class UpdateAppointmentRequest
{
    public DateOnly? Date { get; set; }
    public int? SlotId { get; set; }
}