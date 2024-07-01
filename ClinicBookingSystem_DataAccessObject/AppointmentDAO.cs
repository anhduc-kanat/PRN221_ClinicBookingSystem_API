using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class AppointmentDAO : BaseDAO<Appointment>
{
    private readonly ClinicBookingSystemContext _dbContext;
    private readonly IBaseDAO<AppointmentBusinessService> _appointmentBusinessServiceDao;

    public AppointmentDAO(ClinicBookingSystemContext dbContext, IBaseDAO<AppointmentBusinessService> appointmentBusinessServiceDao) : base(dbContext)
    {
        _dbContext = dbContext;
        _appointmentBusinessServiceDao = appointmentBusinessServiceDao;
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointment()
    {
        return await GetAllAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .ToListAsync();
    }

    public async Task<Appointment> GetAppointmentById(int Id)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .Include(p => p.AppointmentBusinessServices)
            .ThenInclude(p => p.BusinessService)
            .FirstOrDefaultAsync(p => p.Id == Id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentPagination(int pageNumber, int pageSize)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentByUserId(int userId, int pageNumber, int pageSize)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .Where(p => p.Users.Any(p => p.Id == userId))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountUserAppointment(int userId)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Where(p => p.Users.Any(p => p.Id == userId))
            .CountAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentByDayPagination(int pageNumber, int pageSize,
        DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day)
            .Where(p => p.IsClinicalExamPaid == true)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> DentistGetTodayAppointment(int pageNumber, int pageSize, int dentistId, DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            .Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)
            .Include(p => p.AppointmentBusinessServices)
            .Where(p => p.Users.Any(p => p.Id == dentistId))
            .Where(p => p.IsClinicalExamPaid == true)
            .Where(p => p.Status == AppointmentStatus.OnGoing)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<int> CountDentistAppointment(int userId, DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Where(p => p.Users.Any(p => p.Id == userId))
            .Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)
            .Where(p => p.Status == AppointmentStatus.OnGoing && p.IsClinicalExamPaid == true)
            .CountAsync();
    }
    public async Task<int> CountWhenStaffGetAppointmentByDate(DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)
            .Where(p => p.IsClinicalExamPaid == true)
            .CountAsync();
    }
}