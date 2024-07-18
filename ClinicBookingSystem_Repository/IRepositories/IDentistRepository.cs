using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Repository.IRepositories
{
    public interface IDentistRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetDentistsByRole();
        Task<IEnumerable<User>> GetDentistsByService(string serviceName);
        Task<IEnumerable<DateTime>> GetAvailableDate(int id);
        Task<User> CreateNewDentist(User user, List<BusinessService> services);
        Task<IEnumerable<User>> GetDentistsByServiceId(int serviceId);
        Task<User> GetDentistById(int dentistId);
        Task<IEnumerable<User>> GetDentistIsNotBusy();
        Task<User> GetDentistByIdAndSpecificationId(int dentistId, int specificationId);
        Task<IEnumerable<User>> GetDentistBySpecificationId(int specificationId);
    }
}