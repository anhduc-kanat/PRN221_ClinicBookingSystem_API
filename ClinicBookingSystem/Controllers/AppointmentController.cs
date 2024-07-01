using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Pagination;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Request.Payment;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using ClinicBookingSystem_Service.ThirdParties.VnPay;
using ClinicBookingSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/appointment")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPaymentService _paymentService;
    private readonly GetUserIpAddress _getUserIpAddress;


    public AppointmentController(IAppointmentService appointmentService,
        IPaymentService paymentService,
        GetUserIpAddress getUserIpAddress)
    {
        _appointmentService = appointmentService;
        _paymentService = paymentService;
        _getUserIpAddress = getUserIpAddress;
    }
    // GET: api/appointment
    [HttpGet]
    [Route("get-all-appointment")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetAppointmentResponse>>>> GetAllAppointments()
    {
        var response = await _appointmentService.GetAllAppointments();
        return Ok(response);
    }

    [HttpGet]
    [Route("get-all-appointment-pagination")]
    public async Task<ActionResult<PaginationResponse<GetAppointmentResponse>>> GetAllAppointmentsPagination([FromQuery] PaginationRequest request)
    {
        var response = await _appointmentService.GetAllAppointmentsPagination(request.PageNumber, request.PageSize);
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
    
    /// <summary>
    /// - Người dùng đặt lịch hẹn với bác sĩ, login và truyền Bearer token vào header
    ///
    /// - Chỉ có role CUSTOMER mới được phép truy cập
    /// </summary>
    /// <remarks>
    /// - patientId: Id của người thân hoặc bản thân người dùng khi đặt lịch
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    // POST: api/appointment/user-booking-appointment
    [HttpPost]
    [Route("user-booking-appointment")]
    [Authorize(Roles ="CUSTOMER")]
    public async Task<ActionResult<BaseResponse<CustomerBookingAppointmentResponse>>>
        UserBookingAppointment([FromBody] CustomerBookingAppointmentRequest request)
    {
        string userIpAddress = _getUserIpAddress.GetIpAddress();
        var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        var appointmentResponse = await _appointmentService.UserBookingAppointment(userId, request);
        UserInfoRequest userInfoRequest = new UserInfoRequest
        {
            UserIpAddress = userIpAddress,
        };
        
        var response = await _paymentService.CreateVnPayPaymentUrl(appointmentResponse.Data.AppointmentId, userInfoRequest);
        return Ok(response);
    }
    
    //POST: api/appointment/staff-booking-appointment
    /// <summary>
    /// - Staff đặt lịch cho customer tại quầy, login và truyền Bearer token vào header
    ///
    /// - Chỉ có role STAFF mới được phép truy cập
    /// </summary>
    /// <remarks>
    /// - userAccountId: Id của người dùng khi đặt lịch
    /// 
    /// - patientId: Id của người thân hoặc bản thân người dùng khi đặt lịch
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("staff-booking-appointment")]
    [Authorize(Roles = "STAFF")]
    public async Task<ActionResult<BaseResponse<StaffBookingAppointmentResponse>>>
        StaffBookingAppointment([FromBody] StaffBookingAppointmentForCustomerRequest request)
    {
        string userIpAddress = _getUserIpAddress.GetIpAddress();
        int staffId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        var appointmentResponse = await _appointmentService.StaffBookingAppointmentForUser(staffId, request);
        UserInfoRequest userInfoRequest = new UserInfoRequest
        {
            UserIpAddress = userIpAddress,
        };
        var response = await _paymentService.CreateVnPayPaymentUrl(appointmentResponse.Data.AppointmentId, userInfoRequest);

        return Ok(response);
    }
    
    /// <summary>
    /// - User get tất cả các cuộc hẹn của bản thân, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// - status:
    /// 
    /// + 0: Cancelled (Bị hủy bởi customer)
    /// 
    /// + 1: Done (Đã hoàn thành cuộc hẹn => tức là khi sinh ra result)
    /// 
    /// + 2: OnGoing (Staff check-in customer, bắt đầu cuộc hẹn)
    /// 
    /// + 3: Scheduled (Hệ thống tự động tạo ra khi customer đặt lịch)
    /// 
    /// + 4: Rejected (Staff hủy cuộc hẹn của customer)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị) 
    /// </remarks>
    /// <param name="paginationRequest"></param>
    /// <returns></returns>
    //GET: api/appointment/user-get-appointment
    [HttpGet]
    [Route("user-get-appointment")]
    [Authorize(Roles="CUSTOMER")]
    public async Task<ActionResult<PaginationResponse<UserGetAppointmentResponse>>> UserGetAppointment([FromQuery] PaginationRequest paginationRequest)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        var response = await _appointmentService.GetAppointmentByUserId(userId, paginationRequest.PageNumber, paginationRequest.PageSize);
        return Ok(response);
    }
    
    /// <summary>
    /// - Staff chỉ được quyền update status của cuộc hẹn là Rejected (4) và OnGoing (2)
    /// </summary>
    /// <remarks>
    /// appointmentStatus:
    /// 
    /// + 0: Cancelled (Bị hủy bởi customer)
    /// 
    /// + 1: Done (Đã hoàn thành cuộc hẹn => tức là khi sinh ra result)
    /// 
    /// + 2: OnGoing (Staff check-in customer, bắt đầu cuộc hẹn)
    /// 
    /// + 3: Scheduled (Hệ thống tự động tạo ra khi customer đặt lịch)
    /// 
    /// + 4: Rejected (Staff hủy cuộc hẹn của customer)
    /// </remarks>
    /// <param name="appointmentId"></param>
    /// <param name="appointmentStatus"></param>
    /// <returns></returns>
    //UPDATE: api/appointment/staff-checkin-customer-appointment
    [HttpPut]
    [Route("staff-update-customer-appointment/{appointmentId}")]
    [Authorize(Roles = "STAFF")]
    public async Task<ActionResult<BaseResponse<StaffUpdateAppointmentStatusResponse>>> StaffCheckinCustomer(int appointmentId, AppointmentStatus appointmentStatus)
    {
        var response = await _appointmentService.StaffUpdateAppointmentStatus(appointmentId, appointmentStatus); 
        return Ok(response);
    }
    
    /// <summary>
    /// - Staff get tất cả appointments theo ngày, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// - status:
    /// 
    /// + 0: Cancelled (Bị hủy bởi customer)
    /// 
    /// + 1: Done (Đã hoàn thành cuộc hẹn => tức là khi sinh ra result)
    /// 
    /// + 2: OnGoing (Staff check-in customer, bắt đầu cuộc hẹn)
    /// 
    /// + 3: Scheduled (Hệ thống tự động tạo ra khi customer đặt lịch)
    /// 
    /// + 4: Rejected (Staff hủy cuộc hẹn của customer)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị) 
    /// </remarks>
    /// <param name="paginationRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("staff-get-appointment-by-date")]
    [Authorize(Roles = "STAFF")]
    public async Task<ActionResult<PaginationResponse<StaffGetAppointmentByDayResponse>>> StaffGetAppointmentByDate([FromQuery] PaginationRequest paginationRequest, [FromQuery] DateOnly date)
    {
        var response = await _appointmentService.StaffGetAllAppointmentByDay(paginationRequest.PageNumber, paginationRequest.PageSize, date);
        return Ok(response);
    }
    
    /// <summary>
    /// - Dentist get tất cả appointments theo ngày, khi staff checkin appointment thì dentist mới nhận được appointment, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// - status:
    /// 
    /// + 0: Cancelled (Bị hủy bởi customer)
    /// 
    /// + 1: Done (Đã hoàn thành cuộc hẹn => tức là khi sinh ra result)
    /// 
    /// + 2: OnGoing (Staff check-in customer, bắt đầu cuộc hẹn)
    /// 
    /// + 3: Scheduled (Hệ thống tự động tạo ra khi customer đặt lịch)
    /// 
    /// + 4: Rejected (Staff hủy cuộc hẹn của customer)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị) 
    /// </remarks>
    /// <param name="paginationRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("dentist-get-appointment-by-date")]
    [Authorize(Roles = "DENTIST")]
    public async Task<ActionResult<PaginationResponse<DentistGetTodayAppointmentsResponse>>> DentistGetAppointmentByDay([FromQuery] PaginationRequest paginationRequest, DateOnly date)
    {
        var dentistId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        var response = await _appointmentService.DentistGetAppointmentByDay(paginationRequest.PageNumber, paginationRequest.PageSize, dentistId, date);
        return Ok(response);
    }
    /// <summary>
    /// Dentist add thêm các services vào appointment, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// Giá trị truyền vào là 1 mảng chứa nhiều các serviceId và 1 mảng meettings. Với meetings, giá trị truyền
    /// vào là 1 mảng chứa các ngày hẹn của 1 service trong cuộc hẹn đó
    /// </remarks>

    /// <param name="appointmentId"></param>
    /// <param name="businessService"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("dentist-add-service-into-appointment/{appointmentId}")]
    public async Task<ActionResult<BaseResponse<DentistAddServiceIntoAppointmentResponse>>> DentistAddServiceIntoAppointment(int appointmentId, 
        [FromBody] IEnumerable<DentistAddServiceIntoAppointmentRequest> businessService)
    {
        var response = await _appointmentService.DentistAddServiceIntoAppointment(appointmentId, businessService);
        return Ok(response);
    }

}