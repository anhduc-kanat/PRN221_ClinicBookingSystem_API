using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class SpecificationRepository : BaseRepository<Specification>, ISpecificationRepository
{
    private readonly SpecificationDAO _specificationDAO;
    public SpecificationRepository(SpecificationDAO specificationDAO) : base(specificationDAO)
    {
        _specificationDAO = specificationDAO;
    }
}