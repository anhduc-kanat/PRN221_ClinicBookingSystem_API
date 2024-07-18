﻿using ClinicBookingSystem_BusinessObject.Entities;
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
            //result
            .Include(p => p.Result)
            .ThenInclude(p => p.Notes)
            .Include(p => p.Result)
            .ThenInclude(p => p.Medicines)
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            //appointment service
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.BusinessService)
            
            .FirstOrDefaultAsync(p => p.Id == Id);
    }

    public async Task<IEnumerable<Appointment>> GetAllAppointmentPagination(int pageNumber, int pageSize)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            //result
            .Include(p => p.Result)
            .ThenInclude(p => p.Notes)
            .Include(p => p.Result)
            .ThenInclude(p => p.Medicines)
            //appointment service            
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false))
            .ThenInclude(p => p.BusinessService)
            
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentByUserId(int userId, int pageNumber, int pageSize)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            //result
            .Include(p => p.Result)
            .ThenInclude(p => p.Medicines)
            .Include(p => p.Result)
            .ThenInclude(p => p.Notes)
            //appointment service
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false))
            .ThenInclude(p => p.BusinessService)
            
            .Where(p => p.Users.Any(p => p.Id == userId))
            .OrderByDescending(p => p.Date)
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
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            //appointment service
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.BusinessService)
            
            /*.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day)*/
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Year == date.Year)) 
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Month == date.Month))
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Day == date.Day)))
            
            .Where(p => p.AppointmentBusinessServices.Any(p =>
                p.IsDelete == false ))
            
            .Where(p => p.IsClinicalExamPaid == true)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<IEnumerable<Appointment>> GetTodayMeetingTreatmentAppointment()
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            //appointment service
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.BusinessService)
            
            /*.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day)*/
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Year == DateTime.Now.Year)) 
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Month == DateTime.Now.Month))
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Day == DateTime.Now.Day)))
            
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Status == MeetingStatus.CheckIn)))
            
            .Where(p => p.AppointmentBusinessServices.Any(p =>
                p.IsDelete == false ))
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> DentistGetTodayAppointment(int pageNumber, int pageSize, int dentistId, DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            //user
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Users)
            .ThenInclude(p => p.UserProfiles)
            /*.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)*/
            
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Year == date.Year)) 
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Month == date.Month))
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Day == date.Day)))*/
            
            //appointment service
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.Meetings)
            .Include(p => p.AppointmentBusinessServices.Where(p => p.IsDelete == false ))
            .ThenInclude(p => p.BusinessService)
            //result
            .Include(p => p.Result)
            .ThenInclude(p => p.Medicines)
            .Include(p => p.Result)
            .ThenInclude(p => p.Notes)
            
            .Where(p => p.AppointmentBusinessServices.Any(abs => abs.Meetings.Any(m => 
                (m.Date.Value.Year == date.Year &&
                 m.Date.Value.Month == date.Month &&
                 m.Date.Value.Day == date.Day && 
                 m.DentistId == dentistId &&
                 ((m.AppointmentBusinessService.ServiceType == (ServiceType?)1 && m.Status.Value == MeetingStatus.CheckIn) ||
                  (m.AppointmentBusinessService.ServiceType == (ServiceType?)2 && m.Status.Value == MeetingStatus.InTreatment
                  ))))))
            /*
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.DentistId == dentistId)))
            */
            .Where(p => p.IsClinicalExamPaid == true)
            /*
            .Where(p => p.Status == AppointmentStatus.OnGoing)
            */
            
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => 
                p.Date.Value.Year == date.Year &&
                p.Date.Value.Month == date.Month &&
                p.Date.Value.Day == date.Day && 
                ((p.AppointmentBusinessService.ServiceType == (ServiceType?)1 && p.Status.Value == MeetingStatus.CheckIn) ||
                 (p.AppointmentBusinessService.ServiceType == (ServiceType?)2 && p.Status.Value == MeetingStatus.InTreatment)))))*/
                
            /*p.Status.Value == MeetingStatus.CheckIn ||
                p.Status.Value == MeetingStatus.InTreatment &&
                p.Status.Value != MeetingStatus.Done)))*/
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p =>
                p.Status.Value != MeetingStatus.Done)))*/
            .Where(p => p.AppointmentBusinessServices.Any(p =>
                p.IsDelete == false ))
            
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
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.DentistId == userId)))
            /*.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)*/
            
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Year == date.Year)) 
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Month == date.Month))
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Day == date.Day)))*/
            .Where(p => p.AppointmentBusinessServices.Any(abs => abs.Meetings.Any(m => 
                (m.Date.Value.Year == date.Year &&
                 m.Date.Value.Month == date.Month &&
                 m.Date.Value.Day == date.Day && 
                 m.DentistId == userId &&
                 ((m.AppointmentBusinessService.ServiceType == (ServiceType?)1 && m.Status.Value == MeetingStatus.CheckIn) ||
                  (m.AppointmentBusinessService.ServiceType == (ServiceType?)2 && m.Status.Value == MeetingStatus.InTreatment
                  ))))))
            /*
            .Where(p => p.Status == AppointmentStatus.OnGoing && p.IsClinicalExamPaid == true)
            */
            
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => 
                p.Date.Value.Year == date.Year &&
                p.Date.Value.Month == date.Month &&
                p.Date.Value.Day == date.Day && 
                ((p.AppointmentBusinessService.ServiceType == (ServiceType?)1 && p.Status.Value == MeetingStatus.CheckIn) ||
                 (p.AppointmentBusinessService.ServiceType == (ServiceType?)2 && p.Status.Value == MeetingStatus.InTreatment)))))*/
                
            /*p.Status.Value == MeetingStatus.CheckIn ||
                p.Status.Value == MeetingStatus.InTreatment &&
                p.Status.Value != MeetingStatus.Done)))*/
                
            /*.Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Status == MeetingStatus.CheckIn)))*/
            .Where(p => p.IsClinicalExamPaid == true)
            .CountAsync();
    }
    public async Task<int> CountWhenStaffGetAppointmentByDate(DateOnly date)
    {
        return await GetQueryableAsync()
            .Include(p => p.Slot)
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            /*.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day)*/
            .Where(p => p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Year == date.Year)) 
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Month == date.Month))
                        && p.AppointmentBusinessServices.Any(p => p.Meetings.Any(p => p.Date.Value.Day == date.Day)))
            .Where(p => p.IsClinicalExamPaid == true)
            .CountAsync();
    }

    public async Task<Appointment> GetAppointmentIfExistTreatmentMeeting(int appointmentId)
    {
        return await GetQueryableAsync()
            .Include(p => p.AppointmentBusinessServices)
            .ThenInclude(p => p.Meetings)
            .Where(p => p.Id == appointmentId)
            .FirstOrDefaultAsync(p => p.AppointmentBusinessServices.Any(
                p => p.Meetings.Any(
                    p => p.Status == MeetingStatus.InQueue || p.Status == MeetingStatus.InTreatment || p.Status == MeetingStatus.CheckIn)));
    }

    public async Task<IEnumerable<Appointment>> CountAppointment(DateTime date, int type)
    {
        IEnumerable<Appointment> appointments = null;
        switch (type)
        {
            case 1:
                appointments = await GetQueryableAsync()
                    .Where(a => a.Date.Date == date.Date && a.IsFullyPaid == true)
                    .ToListAsync();
                break;
            case 2:
                appointments = await GetQueryableAsync()
                    .Where(a => a.Date.Month == date.Month && a.Date.Year == date.Year && a.IsFullyPaid == true)
                    .ToListAsync();
                break;
            case 3:
                appointments = await GetQueryableAsync()
                    .Where(a => a.Date.Year == date.Year && a.IsFullyPaid == true)
                    .ToListAsync();
                break;
        }
        return appointments;

    }
}