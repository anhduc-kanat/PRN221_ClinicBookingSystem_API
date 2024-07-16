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
    public class DentistRepository : BaseRepository<User>, IDentistRepository
    {
        private readonly DentistDAO _dentistDAO;

        public DentistRepository(DentistDAO dentistDAO) : base(dentistDAO)
        {
            _dentistDAO = dentistDAO;
        }

        public async Task<User> CreateNewDentist(User user, List<BusinessService> services)
        {
            return await _dentistDAO.CreateNewDentist(user, services);
        }

        public async Task<IEnumerable<User>> GetDentistsByServiceId(int serviceId)
        {
            return await _dentistDAO.GetDentistsByServiceId(serviceId);
        }

        public async Task<User> GetDentistById(int dentistId)
        {
            return await _dentistDAO.GetDentistById(dentistId);
        }

        public async Task<IEnumerable<User>> GetDentistIsNotBusy()
        {
            return await _dentistDAO.GetDentistIsNotBusy();
        }

        public async Task<IEnumerable<DateTime>> GetAvailableDate(int id)
        {
            return await _dentistDAO.GetAllFreeDaysOfDentist(id);
        }

        public async Task<IEnumerable<User>> GetDentistsByRole()
        {
            return await _dentistDAO.GetDentistsByRole();
        }

        public async Task<IEnumerable<User>> GetDentistsByService(string serviceName)
        {
            return await _dentistDAO.GetDentistByService(serviceName);

        }


    }
}