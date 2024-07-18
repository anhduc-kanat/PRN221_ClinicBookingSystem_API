using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Repository.IRepositories
{
    public interface ICustomerRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllCustomer(int roleId);
        Task<User> GetCustomerById(int roleId, int customerId);
        Task<bool> GetCustomerByPhone(string phone);
        Task<bool> GetUserByEmail(string email);
    }
}