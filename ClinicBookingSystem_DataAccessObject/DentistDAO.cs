﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_DataAccessObject
{
    public class DentistDAO : BaseDAO<User>
    {
        private readonly ClinicBookingSystemContext _context;
        private readonly IBaseDAO<Meeting> _meetingDao;
        private readonly IBaseDAO<Appointment> _appointmentDao;
        private readonly IBaseDAO<AppointmentBusinessService> _appointmentBusinessDao;

        public DentistDAO(ClinicBookingSystemContext context, IBaseDAO<Meeting> meetingDao
            , IBaseDAO<Appointment> appointmentDao
            , IBaseDAO<AppointmentBusinessService> appointmentBusinessDao) : base(context)
        {
            _context = context;
            _meetingDao = meetingDao;
            _appointmentDao = appointmentDao;
            _appointmentBusinessDao = appointmentBusinessDao;
        }


        public async Task<User> CreateNewDentist(User user, List<BusinessService>services)
        {

              await _context.Users.AddAsync(user);
              user.BusinessServices = services;
              return user;
        }

        public async Task<IEnumerable<User>> GetDentistsByRole()
        {
            return await GetQueryableAsync()
                .Include(u => u.Role)
                .Include(u => u.BusinessServices)
                .Include(p => p.Specifications)
                .Where(u => u.Role.Name == "DENTIST")
                .ToListAsync();
        }
        public async Task<User> GetDentistById(int dentistId)
        {
            return await GetQueryableAsync()
                .Include(u => u.Role)
                .Include(u => u.BusinessServices)
                .Include(p => p.Specifications)
                .Where(u => u.Id == dentistId)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetDentistByService(string serviceName)
        {

            return await GetQueryableAsync()
                .Include(s => s.BusinessServices)
                .Where(a => a.BusinessServices.Any(b=>b.Name.ToLower() == serviceName.ToLower()) && a.Role.Name =="DENTIST")
                .ToListAsync();
        }
        public async Task<IEnumerable<DateTime>> GetAllFreeDaysOfDentist(int id)
        {
            var today = DateTime.Today;
            
            var endDay = today.AddMonths(2);

            IEnumerable<DateTime> allDays = Enumerable.Range(0, (endDay - today).Days + 1)
                                .Select(d => today.AddDays(d))
                                .ToList();
            int slot = await _context.Slots.CountAsync();

    /*        var bookedDates = await _context.Meetings
                .Where(a => a.Date >= today && a.Date <= endDay)
                .GroupBy(a => a.Date.Value.Date)
                .Where(g => g.Count() >= slot)
                .Select(a => a.Key)
                .ToListAsync();*/

            var bookedDates = await _appointmentDao.GetQueryableAsync()
                .Where(a => a.Date >= today && a.Date <= endDay && a.IsClinicalExamPaid == true && _appointmentBusinessDao.GetQueryableAsync()
                .Where(abs => abs.Appointment.Id == a.Id && _meetingDao.GetQueryableAsync()
                    .Where(m => m.DentistId == id)
                    .Select(m => m.AppointmentBusinessService.Id)
                    .Contains(abs.Id))
                    .Any())
                    .GroupBy(a => a.Date.Date)
                    .Where(g => g.Count() >= slot)
                    .Select(g => g.Key)
                    .ToListAsync();


            return bookedDates;
        }

        public async Task<IEnumerable<User>> GetDentistsByServiceId(int serviceId)
        {
            return await GetQueryableAsync()
                .Include(s => s.BusinessServices)
                .Include(p => p.Specifications)
                //.Where(a => a.BusinessServices.Any(p => p.Id == serviceId && a.Role.Name =="DENTIST"))
                .Where(p => p.Role.Name == "DENTIST" && p.Specifications.Any(p => p.BusinessServices.Any(p => p.Id == serviceId)))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetDentistIsNotBusy()
        {
            return await GetQueryableAsync()
                .Include(s => s.BusinessServices.Where(p => p.ServiceType == ServiceType.Treatment))
                .Include(p => p.Specifications)
                .Where(a => a.IsBusy == false && a.Role.Name == "DENTIST")
                .ToListAsync();
        }

        public async Task<User> GetDentistByIdAndSpecificationId(int dentistId, int specificationId)
        {
            return await GetQueryableAsync()
                .Include(p => p.Specifications)
                .Include(p => p.BusinessServices)
                .FirstOrDefaultAsync(p => p.Id == dentistId && p.Specifications.Any(s => s.Id == specificationId));
        }

        public async Task<IEnumerable<User>> GetDentistBySpecificationId(int specificationId)
        {
            return await GetQueryableAsync()
                .Include(p => p.Specifications)
                .Where(p => p.Specifications.Any(p => p.Id == specificationId))
                .ToListAsync();
        }
    }

}