using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Repository.Repositories
{
    public class CustomerRepository : BaseRepository<User>, ICustomerRepository
    {
        private readonly CustomerDAO _customerDAO;
        public CustomerRepository(CustomerDAO customerDAO): base(customerDAO) 
        {
            _customerDAO = customerDAO;
        }

        public Task<IEnumerable<User>> GetAllCustomer(int roleId)
        {
            return _customerDAO.GetAllCustomer(roleId);
        }

        public Task<User> GetCustomerById(int roleId, int customerId)
        {
            return _customerDAO.GetCustomerById(roleId,customerId);

        }

        public Task<bool> GetCustomerByPhone(string phone)
        {
            return _customerDAO.GetUserByPhone(phone);
        }

        public async Task<bool> GetUserByEmail(string email)
        {
            return await _customerDAO.GetUserByEmail(email);
        }
    }
}