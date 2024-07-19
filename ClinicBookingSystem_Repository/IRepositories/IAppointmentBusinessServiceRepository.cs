using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface IAppointmentBusinessServiceRepository : IBaseRepository<AppointmentBusinessService>
{
    Task<IEnumerable<AppointmentBusinessService>> GetAppointmentBusinessServiceByAppointmentId(int appointmentId);

    Task<AppointmentBusinessService> GetAppointmentBusinessServiceByDentistInThatTask(int appointmentBusinessServiceId);

    Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusiness(int appointmentId);
    Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusinessServiceByAppointmentId(int appointmentId);
    Task<IEnumerable<KeyValuePair<string, int>>> CountUseService();

}