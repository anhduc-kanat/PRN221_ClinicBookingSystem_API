using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    private readonly AppointmentDAO _appointmentDAO;
    public AppointmentRepository(AppointmentDAO appointmentDAO) : base(appointmentDAO)
    {
        _appointmentDAO = appointmentDAO;
    }
}