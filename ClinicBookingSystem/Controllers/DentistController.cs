﻿using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.Request.Dentist;
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
        private readonly IQueueService _queueService;
        public DentistController(IDentistService dentistService,
            IQueueService queueService)
        {
            _dentistService = dentistService;
            _queueService = queueService;
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
            response.Data.QueueLength = await _queueService.GetQueueLength(response.Data.PhoneNumber);
            return response;
        }

        [HttpGet("get-dentists")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllDentistsResponse>>>> GetAllDentists()
        {
            var response = await _dentistService.GetAllDentists();
            foreach (var res in response.Data)
            {
                res.QueueLength = await _queueService.GetQueueLength(res.PhoneNumber);
            }
            return response;
        }

        [HttpDelete("delete-dentist/{id}")]
        public async Task<ActionResult<BaseResponse<DeleteDentistResponse>>> DeleteDentist(int id)
        {
            var response = await _dentistService.DeleteDentist(id);
            return response;
        }

        [HttpGet("get-dentist-service")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllDentistsResponse>>>> GetDentistByService(string serviceName)
        {
            var response = await _dentistService.GetAllDentistsByService(serviceName);
            return response;
        }
        
        /// <summary>
        /// Lấy tất cả các bác sĩ theo serviceId
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-dentist-service/{businessServiceId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllDentistsResponse>>>> GetDentistByServiceId(int businessServiceId)
        {
            var response = await _dentistService.GetAllDentistsByServiceId(businessServiceId);
            return response;
        }
        
        [HttpGet("get-date")]
        public async Task<ActionResult<BaseResponse<IEnumerable<DateTime>>>> GetAvailableDateOfDentist(int id)
        {
            var response = await _dentistService.GetAvailableDate(id);
            return response;
        }
        
        /// <summary>
        /// Add dentist vô service
        /// </summary>
        /// <param name="dentistId"></param>
        /// <param name="businessServiceId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-dentist-to-service/{dentistId}/{businessServiceId}")]
        public async Task<ActionResult<BaseResponse<AddDentistToBusinessServiceResponse>>>
            AddDentistToService(int dentistId, int businessServiceId)
        {
            var response = await _dentistService.AddDentistToService(dentistId, businessServiceId);
            return response;
        }
        
        
        /// <summary>
        /// Update specification cho dentist
        /// </summary>
        /// <remarks>
        /// Có thể add thêm nhiều specifications cho và có thể remove nhiều specifications cho 1 dentist.
        ///
        /// Lưu ý nếu không có specification nào thì sẽ remove hết tất cả specification của dentist đó.
        /// </remarks>
        /// <param name="dentistId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-dentist-and-specification/{dentistId}")]
        public async Task<ActionResult<BaseResponse<AddDentistToSpecificationResponse>>> UpdateDentistAndSpecification(int dentistId, 
            UpdateDentistAndSpecificationRequest request)
        {
            var response = await _dentistService.UpdateDentistAndSpecification(dentistId, request);
            return Ok(response);
        }
        
        
    }
}