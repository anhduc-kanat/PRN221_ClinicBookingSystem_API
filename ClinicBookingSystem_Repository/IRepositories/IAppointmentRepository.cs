using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface IAppointmentRepository : IBaseRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAllAppointment();
    Task<Appointment> GetAppointmentById(int Id);
    Task<IEnumerable<Appointment>> GetAllAppointmentPagination(int pageNumber, int pageSize);
    Task<IEnumerable<Appointment>> GetAppointmentByUserId(int userId, int pageNumber, int pageSize);
    Task<int> CountUserAppointment(int userId);
    Task<IEnumerable<Appointment>> GetAppointmentByDatePagination(int pageNumber, int pageSize, DateOnly date);
    Task<IEnumerable<Appointment>> DentistGetTodayAppointment(int pageNumber, int pageSize, int dentistId, DateOnly date);
    Task<int> CountDentistAppointment(int dentistId, DateOnly date);
    Task<int> CountWhenStaffGetAppointmentByDate(DateOnly date);
    Task<IEnumerable<Appointment>> GetTodayMeetingTreatmentAppointment();
    Task<Appointment> GetAppointmentIfExistTreatmentMeeting(int appointmentId);
    Task<IEnumerable<Appointment>> CountAppointment(DateTime date, int type);

}