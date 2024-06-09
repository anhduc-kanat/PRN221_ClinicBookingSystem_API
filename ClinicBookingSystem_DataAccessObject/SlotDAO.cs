using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject
{
    public class SlotDAO : BaseDAO<Slot>
    {
        private readonly ClinicBookingSystemContext _context;

        public SlotDAO(ClinicBookingSystemContext context) : base(context)
        {
            _context = context;
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

    }
}
