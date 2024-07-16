﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;

namespace ClinicBookingSystem_DataAccessObject
{
    public class SlotDAO : BaseDAO<Slot>
    {
        private readonly ClinicBookingSystemContext _context;
        private readonly IBaseDAO<Appointment> _appointmentDao;
        private readonly IBaseDAO<AppointmentBusinessService> _appointmentBusinessDao;
        private readonly IBaseDAO<Meeting> _meetingDao;



        public SlotDAO(ClinicBookingSystemContext context, IBaseDAO<Appointment> appointmentDao
            , IBaseDAO<AppointmentBusinessService> appointmentBusinessDao
            , IBaseDAO<Meeting> meetingDao) : base(context)
        {
            _context = context;
            _appointmentDao = appointmentDao;
            _appointmentBusinessDao = appointmentBusinessDao;
            _meetingDao = meetingDao;
        }

        public async Task<IEnumerable<Slot>> GetAllSlots()
        {
            return await _context.Slots.ToListAsync();
        }
        //
        public async Task<Slot> GetSlotById(int id)
        {
            var slot = await _context.Slots.FindAsync(id);

            return slot;
        }
        //
        public async Task<Slot> CreateSlot(Slot slot)
        {
            _context.Slots.Add(slot);
            await _context.SaveChangesAsync();

            return slot;
        }

        public async Task<Slot> UpdateSlot(Slot slot)
        { 
            var existingSlot = await GetSlotById(slot.Id);
            _context.Slots.Update(existingSlot);
            await _context.SaveChangesAsync();
            return existingSlot;
        }
        //
        public async Task<Slot> DeleteSlot(int id)
        {
            var existingSlot = await GetSlotById(id);
            _context.Slots.Remove(existingSlot);
            await _context.SaveChangesAsync();
            return existingSlot;
        }


        public async Task<IEnumerable<Slot>> CheckAvailableSlot(int dentistId, DateTime dateTime)
        {
            var targetStatus = AppointmentStatus.Scheduled;
            var slots = GetQueryableAsync()
                                  .Where(s => !_appointmentDao.GetQueryableAsync()
                                  .Where( a => a.Slot.Id == s.Id && a.Date.Date == dateTime.Date && a.IsClinicalExamPaid == true)
                                     /* .Include(b => b.AppointmentBusinessServices)*/ // Bao gồm bảng liên kết AppointmentUser
                                      .Any(a => _appointmentBusinessDao.GetQueryableAsync()
                                      .Where(abs => abs.Appointment.Id == a.Id)
                                      .Any(abs => _meetingDao.GetQueryableAsync()
                                      .Where(m => m.DentistId == dentistId)
                                      .Select(m => m.AppointmentBusinessService.Id)
                                      .Contains(abs.Id)
                                      ))).ToList();
            return slots;
        }
        
        public async Task<Slot> GetSlotByTime(TimeSpan startTime, TimeSpan endTime)
        {
            var slot = await GetQueryableAsync()
                .FirstOrDefaultAsync(s => 
                    (startTime >= s.StartAt && startTime < s.EndAt) || 
                    (endTime > s.StartAt && endTime <= s.EndAt));
            return slot;
        }
        public async Task<Slot> GetSlotByTimeExceptCurrentSlot(int slotId, TimeSpan startTime, TimeSpan endTime)
        {
            var slot = await GetQueryableAsync()
                .FirstOrDefaultAsync(s => s.Id != slotId &&
                    ((startTime >= s.StartAt && startTime < s.EndAt) || 
                    (endTime > s.StartAt && endTime <= s.EndAt)));
            return slot;
        }
    }
}