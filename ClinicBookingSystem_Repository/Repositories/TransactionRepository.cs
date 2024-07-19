using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    private readonly TransactionDAO _transactionDAO;
    public TransactionRepository(TransactionDAO transactionDAO) : base(transactionDAO)
    {
        _transactionDAO = transactionDAO;
    }
    public async Task<Transaction> GetTransactionByAppointmentId(int appointmentId)
    {
        return await _transactionDAO.GetTransactionByAppointmentId(appointmentId);
    }

    public async Task<IEnumerable<Transaction>> GetListTransactionByAppointmentId(int appointmentId)
    {
        return await _transactionDAO.GetListTransactionByAppointmentId(appointmentId);
    }

    public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
    {
        return await _transactionDAO.GetTransactionByTransactionId(transactionId);
    }

    public async Task<IEnumerable<Transaction>> GetListTransactionByUserId(int userId)
    {
        return await _transactionDAO.GetTransactionByUser(userId);

    }

    public async Task<IEnumerable<Transaction>> GetAllTransaction()
    {
        return await _transactionDAO.GetAllTransaction();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByDate(DateTime date)
    {
        return await _transactionDAO.GetTransactionByDate(date);
    }
}