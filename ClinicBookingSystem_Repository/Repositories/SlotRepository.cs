using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;

namespace ClinicBookingSystem_Repository.Repositories
{
    public class SlotRepository : BaseRepository<Slot>, ISlotRepository
    {
        private readonly SlotDAO _slotDAO;

        public SlotRepository(SlotDAO slotDao) : base(slotDao)
        {
            _slotDAO ??= slotDao;
        }

        public async Task<IEnumerable<Slot>> GetAllSlots()
        {
            return await _slotDAO.GetAllSlots();
        }

        public async Task<Slot> GetSlotById(int id)
        {
            return await _slotDAO.GetSlotById(id);
        }

        public async Task<Slot> CreateSlot(Slot slot)
        {
            return await _slotDAO.CreateSlot(slot);
        }

        public async Task<Slot> DeleteSlot(int id)
        {
            return await _slotDAO.DeleteSlot(id);
        }

        public async Task<Slot> UpdateSlot(Slot slot)
        {
            return await _slotDAO.UpdateSlot(slot);
        }

        public async Task<IEnumerable< Slot>> CheckAvailableSlot(int dentistId, DateTime dateTime)
        {
            return await _slotDAO.CheckAvailableSlot(dentistId,dateTime);

        }

        public async Task<Slot> GetSlotByTime(TimeSpan startTime, TimeSpan endTime)
        {
            return await _slotDAO.GetSlotByTime(startTime, endTime);
        }
        
        public async Task<Slot> GetSlotByTimeExceptCurrentSlot(int slotId, TimeSpan startTime, TimeSpan endTime)
        {
            return await _slotDAO.GetSlotByTimeExceptCurrentSlot(slotId, startTime, endTime);
        }
    }
}