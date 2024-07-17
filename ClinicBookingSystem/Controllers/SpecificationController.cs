using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Salary;
using ClinicBookingSystem_Service.Models.Request.Specification;
using ClinicBookingSystem_Service.Models.Response.Salary;
using ClinicBookingSystem_Service.Models.Response.Specification;
using ClinicBookingSystem_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/specification")]
public class SpecificationController : ControllerBase
{
    private readonly ISpecificationService _specificationService;

    public SpecificationController(ISpecificationService specificationService)
    {
        _specificationService = specificationService;
    }

    [HttpGet]
    [Route("get-all-specifications")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetSpecificationResponse>>>> GetSpecifications()
    {
        var specifications = await _specificationService.GetAllSpecifications();
        return Ok(specifications);
    }

    [HttpGet]
    [Route("get-specification-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetSpecificationResponse>>> GetSalary(int id)
    {
        var specification = await _specificationService.GetSpecificationById(id);
        return Ok(specification);
    }

    /// <summary>
    /// Tạo collection Specification chứa các BusinessServices
    /// </summary>
    /// <remarks>
    /// BusinessServiceId là một collection chứa các id của BusinessService
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create-specification")]
    public async Task<ActionResult<BaseResponse<CreateSpecificationResponse>>> AddSpecification([FromBody] CreateSpecificationRequest request)
    {
        var createdSpecification = await _specificationService.CreateSpecification(request);
        return Ok(createdSpecification);
    }

    [HttpPut]
    [Route("update-specification/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateSpecificationResponse>>> UpdateSpecification(int id, [FromBody] UpdateSpecificationRequest request)
    {
        var result = await _specificationService.UpdateSpecification(id, request);
        return Ok(result);
    }

    [HttpDelete]
    [Route("delete-specification/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteSpecificationResponse>>> DeleteSpecification(int id)
    {
        var result = await _specificationService.DeleteSpecification(id);
        return Ok(result);
    }
}