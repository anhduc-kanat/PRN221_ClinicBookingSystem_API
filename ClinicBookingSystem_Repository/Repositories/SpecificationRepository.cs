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

    public async Task<Specification> CreateSpecification(Specification specification)
    {
        return await _specificationDAO.CreateSpecification(specification);
    }

    public async Task<Specification> DeleteSpecification(int id)
    {
        return await _specificationDAO.DeleteSpecification(id);
    }

    public async Task<IEnumerable<Specification>> GetAllSpecifications()
    {
        return await _specificationDAO.GetAllSpecifications();
    }

    public async Task<Specification> GetSpecificationById(int id)
    {
        return await _specificationDAO.GetSpecificationById(id);
    }

    public async Task<Specification> UpdateSpecification(Specification specification)
    {
        return await _specificationDAO.UpdateSpecification(specification);
    }
}