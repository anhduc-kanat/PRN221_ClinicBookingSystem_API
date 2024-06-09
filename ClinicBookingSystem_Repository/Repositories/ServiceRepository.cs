using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    private readonly ServiceDAO _serviceDAO;
    public ServiceRepository(ServiceDAO serviceDAO) : base(serviceDAO)
    {
        _serviceDAO = serviceDAO;
    }
    
}