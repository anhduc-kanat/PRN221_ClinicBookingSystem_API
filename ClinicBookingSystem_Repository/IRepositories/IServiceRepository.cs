using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface IServiceRepository : IBaseRepository<BusinessService>
{
    public Task<IEnumerable<BusinessService>> GetAllServices();
    public Task<IEnumerable<BusinessService>> GetAllExamServices();
    public Task<BusinessService> GetServiceById(int id);
    Task<IEnumerable<BusinessService>> GetAllTreatmentServices();
    public Task<BusinessService> CreateService(BusinessService businessService);
    public Task<BusinessService> UpdateService(BusinessService businessService);
    public Task<BusinessService> DeleteService(int id);
    Task<IEnumerable<BusinessService>> GetServicesBySpecification(int specificationId);
}