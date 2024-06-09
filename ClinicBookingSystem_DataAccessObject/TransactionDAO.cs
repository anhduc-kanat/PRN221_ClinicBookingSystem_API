using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;

namespace ClinicBookingSystem_DataAccessObject;

public class TransactionDAO : BaseDAO<Transaction>
{
    private readonly ClinicBookingSystemContext _dbContext;
    public TransactionDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}