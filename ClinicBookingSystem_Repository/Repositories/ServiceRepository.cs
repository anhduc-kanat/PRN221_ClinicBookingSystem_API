using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class ServiceRepository : BaseRepository<BusinessService>, IServiceRepository
{
    private readonly ServiceDAO _serviceDAO;
    public ServiceRepository(ServiceDAO serviceDAO) : base(serviceDAO)
    {
        _serviceDAO = serviceDAO;
    }

    public async Task<IEnumerable<BusinessService>> GetAllTreatmentServices()
    {
        return await _serviceDAO.GetAllTreatmentServices();
    }

    public async Task<BusinessService> CreateService(BusinessService businessService)
    {
        return await _serviceDAO.CreateService(businessService);
    }

    public async Task<BusinessService> DeleteService(int id)
    {
        return await _serviceDAO.DeleteService(id);
    }

    public async Task<IEnumerable<BusinessService>> GetServicesBySpecification(int specificationId)
    {
        return await _serviceDAO.GetServicesBySpecification(specificationId);
    }

    public async Task<IEnumerable<BusinessService>> GetAllExamServices()
    {
        return await _serviceDAO.GetAllExamServices();
    }

    public async Task<IEnumerable<BusinessService>> GetAllServices()
    {
        return await _serviceDAO.GetAllServices();
    }

    public async Task<BusinessService> GetServiceById(int id)
    {
        return await _serviceDAO.GetServiceById(id);
    }

    public async Task<BusinessService> UpdateService(BusinessService businessService)
    {
        return await _serviceDAO.UpdateService(businessService);
    }
    
}