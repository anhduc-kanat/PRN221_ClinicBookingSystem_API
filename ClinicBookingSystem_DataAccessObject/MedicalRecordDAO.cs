using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;

namespace ClinicBookingSystem_DataAccessObject;

public class MedicalRecordDAO : BaseDAO<MedicalRecord>
{
    private readonly ClinicBookingSystemContext _dbContext;
    public MedicalRecordDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}