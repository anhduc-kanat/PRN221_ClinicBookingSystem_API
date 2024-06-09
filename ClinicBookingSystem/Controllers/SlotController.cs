using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Slot;
using ClinicBookingSystem_Service.Models.Response.Slot;
using ClinicBookingSystem_Service.Models.Response.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<Slot>>>> GetSlots()
        {
            var slots = await _slotService.GetAllSlots();
            return Ok(slots);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<SlotResponse>>> GetSlot(int id)
        {
            var slot = await _slotService.GetSlotById(id);
            
            return Ok(slot);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<SlotResponse>>> AddSlot([FromBody] CreateNewSlotRequest request)
        {
            var createdSlot = await _slotService.CreateSlot(request);
            return createdSlot;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<SlotResponse>>> UpdateSlot(int id,[FromBody] UpdateSlotRequest request)
        {
            return await _slotService.UpdateSlot(id, request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<SlotResponse>>> DeleteSlot(int id)
        {
            return await _slotService.DeleteSlot(id);
        }
    }
}

