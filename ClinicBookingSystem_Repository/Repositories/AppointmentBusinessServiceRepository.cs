using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class AppointmentBusinessServiceRepository : BaseRepository<AppointmentBusinessService>, IAppointmentBusinessServiceRepository
{
    private readonly AppointmentBusinessServiceDAO _appointmentBusinessServiceDAO;
    public AppointmentBusinessServiceRepository(AppointmentBusinessServiceDAO appointmentBusinessServiceDAO) : base(appointmentBusinessServiceDAO)
    {
        _appointmentBusinessServiceDAO = appointmentBusinessServiceDAO;
    }



    public async Task<IEnumerable<AppointmentBusinessService>> GetAppointmentBusinessServiceByAppointmentId(int appointmentId)
    {
        return await _appointmentBusinessServiceDAO.GetAppointmentBusinessServiceByAppointmentId(appointmentId);
    }

    public async Task<AppointmentBusinessService> GetAppointmentBusinessServiceByDentistInThatTask(int appointmentBusinessServiceId)
    {
        return await _appointmentBusinessServiceDAO.GetAppointmentBusinessServiceById(appointmentBusinessServiceId);
    }
    public async Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusiness(int appointmentId)
    {
        return await _appointmentBusinessServiceDAO.GetUnPaidAppointmentBusiness(appointmentId);
    }

    public async Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusinessServiceByAppointmentId(int appointmentId)
    {
return await _appointmentBusinessServiceDAO.GetUnPaidAppointmentBusinessServiceByAppointmentId(appointmentId);
    }

    public Task<IEnumerable<KeyValuePair<string, int>>> CountUseService()
    {
        return _appointmentBusinessServiceDAO.CountUseService();
    }
}