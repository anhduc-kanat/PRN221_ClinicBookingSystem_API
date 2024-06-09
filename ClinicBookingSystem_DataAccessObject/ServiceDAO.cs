using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;

namespace ClinicBookingSystem_DataAccessObject;

public class ServiceDAO : BaseDAO<Service>
{
    private readonly ClinicBookingSystemContext _context;
    public ServiceDAO(ClinicBookingSystemContext context) : base(context)
    {
        _context = context;
    }
}