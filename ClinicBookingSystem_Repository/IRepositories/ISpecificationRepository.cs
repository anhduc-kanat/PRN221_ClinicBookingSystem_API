using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface ISpecificationRepository : IBaseRepository<Specification>
{
    public Task<IEnumerable<Specification>> GetAllSpecifications();
    public Task<Specification> GetSpecificationById(int id);
    public Task<Specification> CreateSpecification(Specification specification);
    public Task<Specification> UpdateSpecification(Specification specification);
    public Task<Specification> DeleteSpecification(int id);
}