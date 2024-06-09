using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class RelativeRepository : BaseRepository<Relative>, IRelativeRepository
{
    private readonly RelativeDAO _relativeDAO;
    public RelativeRepository(RelativeDAO relativeDAO) : base(relativeDAO)
    {
        _relativeDAO = relativeDAO;
    }
}