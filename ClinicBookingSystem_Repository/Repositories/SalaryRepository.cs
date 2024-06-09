using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class SalaryRepository : BaseRepository<Salary>, ISalaryRepository
{
    private readonly SalaryDAO _salaryDao;
    public SalaryRepository(SalaryDAO salaryDao) : base(salaryDao)
    {
        _salaryDao = salaryDao;
    }
}