using ClinicBookingSystem_Service.Models.Request.Meeting;

namespace ClinicBookingSystem_Service.Models.Request.Appointment;

public class DentistAddServiceIntoAppointmentRequest
{
    public int BusinessServiceId { get; set; }
    public IEnumerable<CreateMeetingRequest> Meetings { get; set; }
}