using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;

namespace ClinicBookingSystem_DataAccessObject;

public class OrderDAO : BaseDAO<Order> 
{
    private readonly ClinicBookingSystemContext _dbContext;
    public OrderDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}