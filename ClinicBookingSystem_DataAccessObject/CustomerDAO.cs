using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_DataAccessObject
{
    public class CustomerDAO: BaseDAO<User>
    {
        private readonly ClinicBookingSystemContext _dbContext;
        public CustomerDAO(ClinicBookingSystemContext dbContext): base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllCustomer(int roleId)
        {
            IEnumerable<User> customers = await _dbContext.Users.Where(a => a.Role.Id == roleId).ToListAsync();
            return customers;
        }
        public async Task<User> GetCustomerById(int roleId, int customerId)
        {
            User customers = await _dbContext.Users.FirstOrDefaultAsync(a => a.Role.Id == roleId && a.Id == customerId);
            return customers;
        }


        public async Task<bool> GetUserByPhone(string phone)
        {
            User customer = await GetQueryableAsync()
                .FirstOrDefaultAsync(a => a.PhoneNumber == phone);
            return customer !=null;
        }

    }
}