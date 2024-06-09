using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/appointment")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    // GET: api/appointment
    [HttpGet]
    [Route("get-all-appointment")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetAppointmentResponse>>>> GetAllAppointments()
    {
        var response = await _appointmentService.GetAllAppointments();
        return Ok(response);
    }
    // GET: api/appointment/1
    [HttpGet]
    [Route("get-appointment-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetAppointmentResponse>>> GetAppointmentById(int id)
    {
        var response = await _appointmentService.GetAppointmentById(id);
        return Ok(response);
    }
    // POST: api/appointment
    [HttpPost]
    [Route("create-appointment")]
    public async Task<ActionResult<BaseResponse<CreateAppointmentResponse>>> CreateAppointment([FromBody] CreateAppointmentRequest request)
    {
        var response = await _appointmentService.CreateAppointment(request);
        return Ok(response);
    }
    // PUT: api/appointment/1
    [HttpPut]
    [Route("update-appointment/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateAppointmentResponse>>> UpdateAppointment(int id, [FromBody] UpdateAppointmentRequest request)
    {
        var response = await _appointmentService.UpdateAppointment(id, request);
        return Ok(response);
    }
    // DELETE: api/appointment/1
    [HttpDelete]
    [Route("delete-appointment/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteAppointmentResponse>>> DeleteAppointment(int id)
    {
        var response = await _appointmentService.DeleteAppointment(id);
        return Ok(response);
    }
}