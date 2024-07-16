﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Slot;
using ClinicBookingSystem_Service.Models.Response.Slot;
using ClinicBookingSystem_Service;
using Microsoft.AspNetCore.Mvc;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.Response.Service;
using ClinicBookingSystem_Service.Models.Request.Service;

namespace ClinicBookingSystem_API.Controllers;

[Route("api/service")]
[ApiController]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;

    public ServiceController(IServiceService serviceService)
    {
        _serviceService = serviceService;
    }

    [HttpGet]
    [Route("get-all-services")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetServiceResponse>>>> GetServices()
    {
        var services = await _serviceService.GetAllServices();
        return Ok(services);
    }

    /// <summary>
    /// Customers chỉ được book và thấy services khám
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-all-exam-services")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetServiceResponse>>>> GetExamServices()
    {
        var services = await _serviceService.GetAllExamServices();
        return Ok(services);
    }

    [HttpGet]
    [Route("get-service-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetServiceResponse>>> GetService(int id)
    {
        var service = await _serviceService.GetServiceById(id);

        return Ok(service);
    }

    [HttpPost]
    [Route("create-service")]
    public async Task<ActionResult<BaseResponse<CreateServiceResponse>>> AddService([FromBody] CreateServiceRequest request)
    {
        var createdService = await _serviceService.CreateService(request);
        return Ok(createdService);
    }

    [HttpPut]
    [Route("update-service/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateServiceResponse>>> UpdateService(int id, [FromBody] UpdateServiceRequest request)
    {
        var service = await _serviceService.UpdateService(id, request);
        return Ok(service);
    }

    [HttpDelete]
    [Route("delete-service/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteServiceResponse>>> DeleteService(int id)
    {
        var service = await _serviceService.DeleteService(id);
        return Ok(service);
    }
    
    /// <summary>
    /// Dentist thêm services điều trị cho customers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("get-all-treatment-services")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetServiceResponse>>>> GetTreatmentServices()
    {
        var services = await _serviceService.GetAllTreatmentServices();
        return Ok(services);
    }
}