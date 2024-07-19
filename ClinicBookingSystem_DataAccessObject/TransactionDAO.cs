using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class TransactionDAO : BaseDAO<Transaction>
{
    private readonly ClinicBookingSystemContext _dbContext;
    private readonly IBaseDAO<Appointment> _appointmentDao;

    public TransactionDAO(ClinicBookingSystemContext dbContext, IBaseDAO<Appointment> appointment) : base(dbContext)
    {
        _dbContext = dbContext;
        _appointmentDao = appointment;
    }

    public async Task<Transaction> GetTransactionByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Appointment.Id == appointmentId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Transaction>> GetListTransactionByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Appointment.Id == appointmentId)
            .Where(p => p.IsPay == false)
            .ToListAsync();
    }
    public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Id == transactionId)
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByUser(int userId)
    {

        return GetQueryableAsync().Include(p=>p.Appointment.AppointmentBusinessServices)
            .Where(p => _appointmentDao.GetQueryableAsync()
                .Where(a => a.UserAccountId == userId).Select(a => a.Id)
                .Contains(p.Appointment.Id)
            )
            .ToList();
    }


    public async Task<IEnumerable<Transaction>> GetAllTransaction()
    {
        return await GetQueryableAsync()
            .Include(p => p.Appointment)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByDate(DateTime date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Appointment)
            .Where(p => p.PayDate.Value.Date == date.Date)
            .ToListAsync();
    }
}