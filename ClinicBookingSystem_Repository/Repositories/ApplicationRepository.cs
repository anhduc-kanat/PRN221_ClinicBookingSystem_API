using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
{
    private readonly ApplicationDAO _applicationDAO;
    public ApplicationRepository(ApplicationDAO applicationDAO) : base(applicationDAO)
    {
        _applicationDAO = applicationDAO;
    }
}