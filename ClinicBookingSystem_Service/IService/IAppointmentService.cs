using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Response.Appointment;

namespace ClinicBookingSystem_Service.IService;

public interface IAppointmentService
{
    Task<BaseResponse<IEnumerable<GetAppointmentResponse>>> GetAllAppointments();
    Task<BaseResponse<GetAppointmentResponse>> GetAppointmentById(int id);
    Task<BaseResponse<CreateAppointmentResponse>> CreateAppointment(CreateAppointmentRequest request);
    Task<BaseResponse<UpdateAppointmentResponse>> UpdateAppointment(int id, UpdateAppointmentRequest request);
    Task<BaseResponse<DeleteAppointmentResponse>> DeleteAppointment(int id);
}