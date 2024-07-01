using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories;

public class MeetingRepository : BaseRepository<Meeting>, IMeetingRepository
{
    private readonly MeetingDAO _meetingDao;
    public MeetingRepository(MeetingDAO meetingDao) : base(meetingDao)
    {
        _meetingDao = meetingDao;
    }
}