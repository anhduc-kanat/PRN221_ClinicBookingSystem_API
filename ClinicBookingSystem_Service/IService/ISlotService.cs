using ClinicBookingSystem_BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Slot;
using ClinicBookingSystem_Service.Models.Response.Slot;

namespace ClinicBookingSystem_Service
{
    public interface ISlotService
    {
        public Task<BaseResponse<IEnumerable<SlotResponse>>> GetAllSlots();
        public Task<BaseResponse<SlotResponse>> GetSlotById(int id);
        public Task<BaseResponse<SlotResponse>> CreateSlot(CreateNewSlotRequest request);
        public Task<BaseResponse<SlotResponse>> DeleteSlot(int id);
        public Task<BaseResponse<SlotResponse>> UpdateSlot(int id, UpdateSlotRequest request);
    }
}
