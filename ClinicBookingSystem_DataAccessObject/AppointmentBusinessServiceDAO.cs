using System.Collections;
using System.Numerics;
using System.Runtime.InteropServices;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class AppointmentBusinessServiceDAO : BaseDAO<AppointmentBusinessService>
{
    private readonly ClinicBookingSystemContext _dbContext;
    public AppointmentBusinessServiceDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<AppointmentBusinessService>> GetAppointmentBusinessServiceByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Include(p => p.BusinessService)
            .Include(p => p.Meetings)
            .Where(p => p.Appointment.Id == appointmentId)
            .ToListAsync();
    }
    public async Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusinessServiceByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Include(p => p.BusinessService)
            .Include(p => p.Meetings)
            .Where(p => p.IsPaid == false || p.IsPaid == null)
            .Where(p => p.Appointment.Id == appointmentId)
            .ToListAsync();
    }
    public async Task<AppointmentBusinessService> GetAppointmentBusinessServiceById(int appointmentBusinessServiceId)
    {
        return await GetQueryableAsync()
            .Include(p => p.BusinessService)
            .Include(p => p.Meetings)
            .Where(p => p.Id == appointmentBusinessServiceId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<AppointmentBusinessService>> GetUnPaidAppointmentBusiness(int appointmentId)
    {
        return await GetQueryableAsync()
            .Include(p => p.Meetings)
            .Include(p => p.Appointment)
            .Where(p => p.Appointment.Id == appointmentId && p.Appointment.IsClinicalExamPaid == true)
            .Where(p => p.IsPaid == false)
            .Where(p => p.Appointment.IsFullyPaid == false || p.Appointment.IsFullyPaid == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<KeyValuePair<string, int>>> CountUseService()
    {
        var result = await GetQueryableAsync()
            .Where(a => a.IsPaid == true && a.ServiceType == ServiceType.Treatment)
            .GroupBy(a => a.ServiceName)
            .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
            .ToListAsync();

        return result;
    }


}