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

        public SlotDAO(ClinicBookingSystemContext context, IBaseDAO<Appointment> appointmentDao) : base(context)
        {
            _context = context;
            _appointmentDao = appointmentDao;
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
                                      .Include(b => b.Users) // Bao gồm bảng liên kết AppointmentUser
                                      .Any(b => b.Slot.Id == s.Id &&
                                                b.Date.Date == dateTime.Date &&
                                                b.Users.Any(c => c.Id == dentistId)
                                                && b.Status == targetStatus))
                                  .ToList();
            return slots;
        }
    }
}
