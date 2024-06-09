using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class ClaimRepository : BaseRepository<Claim>, IClaimRepository
{
    private readonly ClaimDAO _claimDAO;
    public ClaimRepository(ClaimDAO claimDAO) : base(claimDAO)
    {
        _claimDAO = claimDAO;
    }
}