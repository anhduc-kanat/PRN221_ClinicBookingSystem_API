using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Role;
using ClinicBookingSystem_Service.Models.Response.Role;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("/api/role")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    [Route("/get-role-by-name/{roleName}")]
    public async Task<ActionResult<BaseResponse<GetRoleResponse>>> GetRoleByName(string roleName)
    {
        var response = await _roleService.GetRoleByName(roleName);
        return Ok(response);
    }
    //Create role
    [HttpPost]
    [Route("/create-role")]
    public async Task<ActionResult<BaseResponse<CreateRoleResponse>>> CreateRole([FromBody] CreateRoleRequest request)
    {
        var response = await _roleService.CreateRole(request);
        return Ok(response);
    }
    //Update role
    [HttpPut]
    [Route("/update-role/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateRoleResponse>>> UpdateRole(int id, [FromBody] UpdateRoleRequest request)
    {
        var response = await _roleService.UpdateRole(id, request);
        return Ok(response);
    }
    //Delete role
    [HttpDelete]
    [Route("/delete-role/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteRoleResponse>>> DeleteRole(int id)
    {
        var response = await _roleService.DeleteRole(id);
        return Ok(response);
    }
    //Get role by id
    [HttpGet]
    [Route("/get-role-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetRoleResponse>>> GetRoleById(int id)
    {
        var response = await _roleService.GetRoleById(id);
        return Ok(response);
    }
    //Get all roles
    [HttpGet]
    [Route("/get-all-roles")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetRoleResponse>>>> GetAllRoles()
    {
        var response = await _roleService.GetAllRoles();
        return Ok(response);
    }
}