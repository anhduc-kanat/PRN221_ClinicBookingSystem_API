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
    public class StaffDAO : BaseDAO<User>
    {
        private readonly ClinicBookingSystemContext _context;
        public StaffDAO(ClinicBookingSystemContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetStaffsByRole()
        {
            return await GetQueryableAsync().Include(u => u.Role).Where(u => u.Role.Name == "STAFF").ToListAsync();
        }
    }
}