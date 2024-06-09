using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class ClinicRepository : BaseRepository<Clinic>, IClinicRepository
{
    private readonly ClinicDAO _clinicDAO;
    public ClinicRepository(ClinicDAO clinicDAO) : base(clinicDAO)
    {
        _clinicDAO = clinicDAO;
    }
}