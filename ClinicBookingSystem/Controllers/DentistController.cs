using ClinicBookingSystem_Service.Dtos.Request;
using ClinicBookingSystem_Service.IServices;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Dentist;
using ClinicBookingSystem_Service.Models.Response.Dentist;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers
{
    [Route("api/dentist")]
    [ApiController]
    public class DentistController : ControllerBase
    {
        private readonly IDentistService _dentistService;
        public DentistController(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [HttpPost]
        [Route("create-dentist")]
        public async Task<ActionResult<BaseResponse<CreateDentistResponse>>> CreateDentist(CreateDentistRequest request)
        {
            var response = await _dentistService.CreateDentist(request);
            return response;
        }

        [HttpPut("update-dentist/{id}")]
        public async Task<ActionResult<BaseResponse<UpdateDentistResponse>>> UpdateDentist(int id, UpdateDentistRequest request)
        {
            var response = await _dentistService.UpdateDentist(id, request);
            return response;
        }

        [HttpGet("get-dentist/{id}")]
        public async Task<ActionResult<BaseResponse<GetDentistByIdResponse>>> GetDentistById(int id)
        {
            var response = await _dentistService.GetDentistById(id);
            return response;
        }

        [HttpGet("get-dentists")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllDentistsResponse>>>> GetAllDentists()
        {
            var response = await _dentistService.GetAllDentists();
            return response;
        }

        [HttpDelete("delete-dentist/{id}")]
        public async Task<ActionResult<BaseResponse<DeleteDentistResponse>>> DeleteDentist(int id)
        {
            var response = await _dentistService.DeleteDentist(id);
            return response;
        }
    }
}
