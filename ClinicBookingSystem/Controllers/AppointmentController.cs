using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Pagination;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Request.Payment;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using ClinicBookingSystem_Service.RabbitMQ.Consumers.Appointment;
using ClinicBookingSystem_Service.RabbitMQ.Events;
using ClinicBookingSystem_Service.RabbitMQ.Events.Appointment;
using ClinicBookingSystem_Service.RabbitMQ.IService;
using ClinicBookingSystem_Service.ThirdParties.VnPay;
using ClinicBookingSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicBookingSystem_Service.Models.Response.AppointmentService;
using ClinicBookingSystem_Service.Notification.EmailNotification;
using ClinicBookingSystem_Service.Notification.EmailNotification.IService;

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
    /// <summary>
    /// API NÀY KHÔNG SỬ DỤNG ĐƯỢC
    /// </summary>
    /// <returns></returns>
    // GET: api/appointment
    [HttpGet]
    [Route("get-all-appointment")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetAppointmentResponse>>>> GetAllAppointments()
    {
        var response = await _appointmentService.GetAllAppointments();
        return Ok(response);
    }

    /// <summary>
    /// Lấy tất cả appointments theo trang
    /// </summary>
    /// <remarks>
    /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// </remarks>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("get-all-appointment-pagination")]
    public async Task<ActionResult<PaginationResponse<GetAppointmentResponse>>> GetAllAppointmentsPagination([FromQuery] PaginationRequest request)
    {
        var response = await _appointmentService.GetAllAppointmentsPagination(request.PageNumber, request.PageSize);
        return Ok(response);
    }
    
    /// <summary>
    /// Lấy appointment theo id
    /// </summary>
    /// <remarks>
    ///   /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// </remarks>
    /// <param name="id"></param>
    /// <returns></returns>
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
        
        var createTransactionResponse = await _paymentService.CreateTransaction(new CreatePaymentTransactionRequest
        {
            AppointmentId = appointmentResponse.Data.AppointmentId,
            Type = TransactionType.BookingType,
            AppointmentBusinessServices = appointmentResponse.Data.AppointmentBusinessServices
        });
        
        CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest
        {
            Type = TransactionType.BookingType,
            ServicePrice = createTransactionResponse.Data.ServicePrice,
            AppointmentId = appointmentResponse.Data.AppointmentId,
            UserIpAddress = userIpAddress,
            UserAccountName = createTransactionResponse.Data.UserAccountName,
            UserAccountPhone = createTransactionResponse.Data.UserAccountPhone
        };
        var response = await _paymentService.CreateVnPayPaymentUrl(createPaymentRequest);
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

        var createTransactionResponse = await _paymentService.CreateTransaction(new CreatePaymentTransactionRequest
        {
            AppointmentId = appointmentResponse.Data.AppointmentId,
            Type = TransactionType.BookingType,
            AppointmentBusinessServices = appointmentResponse.Data.AppointmentBusinessServices,
        });
        
        CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest
        {
            Type = TransactionType.BookingType,
            ServicePrice = createTransactionResponse.Data.ServicePrice,
            AppointmentId = appointmentResponse.Data.AppointmentId,
            UserIpAddress = userIpAddress,
            UserAccountName = createTransactionResponse.Data.UserAccountName,
            UserAccountPhone = createTransactionResponse.Data.UserAccountPhone
        };
        var response = await _paymentService.CreateVnPayPaymentUrl(createPaymentRequest);

        return Ok(response);
    }
    
    /// <summary>
    /// - User get tất cả các cuộc hẹn của bản thân, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// .Staff chỉ cần update meeting status thành CheckIn
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
    /// + 2: OnExamination (Staff check-in customer, bắt đầu cuộc hẹn)
    /// 
    /// + 3: Scheduled (Hệ thống tự động tạo ra khi customer đặt lịch)
    /// 
    /// + 4: Rejected (Staff hủy cuộc hẹn của customer)
    ///
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// .Hiện tại không cân sử dụng api này, checkin bằng meeting
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
    /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// .Staff chỉ cần update meeting status thành CheckIn
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
    /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// .Staff chỉ cần update meeting status thành CheckIn
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
    
    [HttpPost]
    [Route("staff-create-treatment-payment/{appointmentId}")]
    [Authorize(Roles = "STAFF")]
    public async Task<ActionResult<BaseResponse<StaffCreateTreatmentPaymentResponse>>>
        StaffCreateTreatmentPayment(int appointmentId)
    {
        string userIpAddress = _getUserIpAddress.GetIpAddress();
        int staffId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
        var appointmentResponse = await _appointmentService.StaffCreateTreatmentPayment(appointmentId, staffId);

        var createTransactionResponse = await _paymentService.CreateTransaction(new CreatePaymentTransactionRequest
        {
            AppointmentId = appointmentResponse.Data.AppointmentId,
            Type = TransactionType.ServiceType,
            AppointmentBusinessServices = appointmentResponse.Data.AppointmentBusinessServices,
        });
        
        CreatePaymentRequest createPaymentRequest = new CreatePaymentRequest
        {
            Type = TransactionType.BookingType,
            ServicePrice = createTransactionResponse.Data.ServicePrice,
            AppointmentId = appointmentResponse.Data.AppointmentId,
            UserIpAddress = userIpAddress,
            UserAccountName = createTransactionResponse.Data.UserAccountName,
            UserAccountPhone = createTransactionResponse.Data.UserAccountPhone
        };
        var response = await _paymentService.CreateVnPayPaymentUrl(createPaymentRequest);
        return Ok(response);
    }
    
    /// <summary>
    /// Job pick tự động, ko cần sử dụng api này
    /// </summary>
    /// <remarks>
    /// - Appointment status:
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
    /// + 5: Pending (Appointment chưa thanh toán lịch khám)
    ///
    /// + 6: OnTreatment (Appointment đang trong quá trình điều trị)
    ///
    /// + 7: Queued (Appointment đang trong hàng chờ)
    ///
    /// + 8: Waiting (Appointment đã hoàn tất thanh toán, chờ để điều trị)
    ///
    /// - serviceType:
    ///
    /// + 1: Examination (Khám bệnh)
    /// 
    /// + 2: Treatment (Điều trị)
    ///
    /// Appointment status staff ko cần thao tác, chỉ cần thao tác ở meeting
    /// 
    /// -------------------------------------------------------------------
    ///
    /// - Meeting status
    /// 
    /// 1: Done (Hoàn thành meeting)
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    ///
    /// 5: InQueue (Đang trong hàng đợi chờ tới lượt chữa trị
    ///
    /// 6: WaitingDentist (Đang chờ bác sĩ - cái này hiện đang ko sử dụng nhưng mà để cho vui)
    ///
    /// 7: InTreatment (Đang trong quá trình chữa trị)
    ///
    /// .Staff chỉ cần update meeting status thành CheckIn
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("get-appointment-by-meeting-day-for-ajax")]
    public async Task<ActionResult<IEnumerable<GetAppointmentByMeetingDayForAjaxResponse>>>
        GetAppointmentByMeetingDayForAjax()
    {
        var response = await _appointmentService.GetAppointmentByMeetingDayForAjax();
        return Ok(response);
    }


    [HttpDelete]
    [Route("delete-business-service-on-appointment/{appBusinessServiceId}")]
    public async Task<ActionResult<DeleteAppointmentServiceResponse>> DeleteServiceInAppointment(int appBusinessServiceId)
    {
        var response = await _appointmentService.DeleteServiceInAppointment(appBusinessServiceId);
        return Ok(response);
    }

    [HttpGet]
    [Route("statistic-appointment")]
    public async Task<ActionResult<CountAppointment>> DeleteServiceInAppointment(DateTime date,int type)
    {
        var response = await _appointmentService.CountAppointment(date,type);
        return Ok(response);
    }
}