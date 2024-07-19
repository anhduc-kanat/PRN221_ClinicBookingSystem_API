using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<Transaction> GetTransactionByAppointmentId(int appointmentId);
    Task<IEnumerable<Transaction>> GetListTransactionByAppointmentId(int appointmentId);
    Task<Transaction> GetTransactionByTransactionId(int transactionId);
    Task<IEnumerable<Transaction>> GetListTransactionByUserId(int userId);
    Task<IEnumerable<Transaction>> GetAllTransaction();
    Task<IEnumerable<Transaction>> GetTransactionByDate(DateTime date);
}