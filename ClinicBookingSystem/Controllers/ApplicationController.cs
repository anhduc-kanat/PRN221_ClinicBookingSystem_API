﻿using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Application;
using ClinicBookingSystem_Service.Models.Response.Application;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers;
[Route("api/application")]
[ApiController]
public class ApplicationController
{
    private readonly IApplicationService _applicationService;
    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }
    [HttpGet]
    public async Task<ActionResult<BaseResponse<IEnumerable<ApplicationResponse>>>> GetAllApplications()
    {
        var user = await _applicationService.GetAllApplications();
        return user;
    }
    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<BaseResponse<ApplicationResponse>>> GetApplicationById(int id)
    {
        return await _applicationService.GetApplicationById(id);
    }
    [HttpPost]
    public async Task<ActionResult<BaseResponse<ApplicationResponse>>> CreateApplication([FromBody] CreateNewApplicationRequest application)
    {
        return await _applicationService.CreateApplication(application);
    }
    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<BaseResponse<ApplicationResponse>>> UpdateApplication(int id, [FromBody] UpdateApplicationRequest application)
    {
        return await _applicationService.UpdateApplication(id, application);
    }
    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult<BaseResponse<ApplicationResponse>>> DeleteApplication(int id)
    {
        return await _applicationService.DeleteApplication(id);
    }
}