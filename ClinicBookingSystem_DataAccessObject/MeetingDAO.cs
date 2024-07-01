using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;

namespace ClinicBookingSystem_DataAccessObject;

public class MeetingDAO : BaseDAO<Meeting>
{
    private readonly ClinicBookingSystemContext _dbContext;
    public MeetingDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}