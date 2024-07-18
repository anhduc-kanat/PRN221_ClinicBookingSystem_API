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

        /// <summary>
        /// Tạo mới 1 dentist, nhập email để send mail cho dentist
        /// </summary>
        /// <remarks>
        /// Khi send mail sẽ bao gồm tài khoản và mật khẩu để đăng nhập, có thể thay đồi mật khẩu sau khi login
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Lấy dentist theo Id, bao gồm queueLength và specification detail của từng dentist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get-dentist/{id}")]
        public async Task<ActionResult<BaseResponse<GetDentistByIdResponse>>> GetDentistById(int id)
        {
            var response = await _dentistService.GetDentistById(id);
            response.Data.QueueLength = await _queueService.GetQueueLength(response.Data.PhoneNumber);
            return response;
        }

        /// <summary>
        /// Lấy tất cả dentist, bao gồm queueLength và specification detail của từng dentist
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Không biết cái này có xài được không, hay là được fetch bên fe hay không. Nhưng tóm lại là KO SỬ DỤNG API NÀY
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Lấy ra những ngày bận của dentist (đã có lịch hẹn)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        
        
        /// <summary>
        /// Lấy danh sách các dentist trong specification
        /// </summary>
        /// <param name="specificationId">
        /// Id của specification, truyền theo param
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-dentist-by-specification/{specificationId}")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllDentistsResponse>>>>
            GetDentistBySpecificationId(int specificationId)
        {
            var response = await _dentistService.GetDentistBySpecificationId(specificationId);
            return response;
        }
    }
}