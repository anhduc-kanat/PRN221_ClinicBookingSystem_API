using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories
{
    public interface ISlotRepository : IBaseRepository<Slot>
    {
        public Task<IEnumerable<Slot>> GetAllSlots();
        public Task<Slot> GetSlotById(int id);
        public Task<Slot> CreateSlot(Slot slot);
        public Task<Slot> UpdateSlot(Slot slot);
        public Task<Slot> DeleteSlot(int id);
    }
}
