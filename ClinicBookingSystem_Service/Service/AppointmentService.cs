using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.DTOs.Appointment;
using ClinicBookingSystem_Service.Models.DTOs.AppointmentBusinessService;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Pagination;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Request.Payment;
using ClinicBookingSystem_Service.Models.Response.Appointment;
namespace ClinicBookingSystem_Service.Service;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseResponse<IEnumerable<GetAppointmentResponse>>> GetAllAppointments()
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointment();
        var result = _mapper.Map<IEnumerable<GetAppointmentResponse>>(appointments);
        return new BaseResponse<IEnumerable<GetAppointmentResponse>>("Get all appointments successfully", StatusCodeEnum.OK_200, result);
    }

    public async Task<PaginationResponse<GetAppointmentResponse>> GetAllAppointmentsPagination(int pageNumber, int pageSize)
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAllAppointmentPagination(pageNumber, pageSize);
        int count = await _unitOfWork.AppointmentRepository.CountAllAsync();
        var result = _mapper.Map<IList<GetAppointmentResponse>>(appointments);
        return new PaginationResponse<GetAppointmentResponse>(
            "Get all appointments successfully",
            StatusCodeEnum.OK_200,
            result,
            pageNumber,
            pageSize,
            count
        );
    }
    public async Task<BaseResponse<GetAppointmentResponse>> GetAppointmentById(int id)
    {
        Appointment appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(id);
        var result = _mapper.Map<GetAppointmentResponse>(appointment);
        return new BaseResponse<GetAppointmentResponse>("Get appointment by id successfully", StatusCodeEnum.OK_200, result);
    }
    
    public async Task<BaseResponse<CreateAppointmentResponse>> CreateAppointment(CreateAppointmentRequest request)
    {
        //Appointment appointment = _mapper.Map<Appointment>(request);
        DateTime date = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, 0, 0, 0);
        User patient = await _unitOfWork.UserRepository.GetByIdAsync(request.PatientId);
        User dentist = await _unitOfWork.DentistRepository.GetByIdAsync(request.DentistId);
        Slot slot = await _unitOfWork.SlotRepository.GetByIdAsync(request.SlotId);
        ICollection<User> users = new List<User>();
        users.Add(patient);
        users.Add(dentist);
        AppointmentDto appointmentDto = new AppointmentDto
        {  
            Date = date,
            IsReExam = request.IsReExam,
            Slot = slot,
            Users = users
        };
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<CreateAppointmentResponse>(appointment);
        return new BaseResponse<CreateAppointmentResponse>("Create appointment successfully", StatusCodeEnum.Created_201, result);
    }

    public async Task<BaseResponse<CustomerBookingAppointmentResponse>> UserBookingAppointment(int userId,
        CustomerBookingAppointmentRequest request)
    {
        Slot slot = await _unitOfWork.SlotRepository.GetByIdAsync(request.SlotId);
        DateTime date = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, slot.StartAt.Hours, slot.StartAt.Minutes, slot.StartAt.Seconds);
        //Account cua nguoi dat
        User userAccount = await _unitOfWork.UserRepository.GetByIdAsync(userId);
        //Nguoi duoc kham
        UserProfile patient = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.PatientId);
        User dentist = await _unitOfWork.DentistRepository.GetByIdAsync(request.DentistId);
        IEnumerable<Slot> dentistAvailableSlot = await _unitOfWork.SlotRepository.CheckAvailableSlot(dentist.Id, date);
        
        //Validate if dentist free at this slot
        if (!dentistAvailableSlot.Any(a => a.Id == slot.Id)) throw new Exception("This slot is unavailable");
        BusinessService businessService = await _unitOfWork.ServiceRepository.GetByIdAsync(request.ServiceId);
        //Add user into list of users
        ICollection<User> users = new List<User>();
        users.Add(userAccount);
        users.Add(dentist);
        AppointmentDto appointmentDto = new AppointmentDto
        {
            Date = date,
            UserTreatmentId = patient.Id,
            UserTreatmentName = patient.FirstName + patient.LastName,
            UserAccountId = userAccount.Id,
            UserAccountName = userAccount.FirstName + userAccount.LastName,
            Slot = slot,
            Users = users
        };
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        //set who is the person booking the appointment and who is the patient
        /*appointment.UserAccountId = userAccount.Id;
        appointment.UserAccountName = userAccount.FirstName + userAccount.LastName;

        appointment.UserTreatmentName = patient.FirstName + patient.LastName;
        appointment.UserTreatmentId = patient.Id;
        
        appointment.DentistTreatmentId = */
        //Set default status for appointment
        appointment.Status = AppointmentStatus.Pending;
        
        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
        
        //Create appointmentBusinessService
        var appointmentBusinessServiceDto = _mapper.Map<AppointmentBusinessServiceDto>(appointment);
        appointmentBusinessServiceDto.Status = AppointmentBusinessServiceStatus.Undone;
        appointmentBusinessServiceDto.DentistId = dentist.Id;
        appointmentBusinessServiceDto.DentistName = dentist.FirstName + dentist.LastName;
        appointmentBusinessServiceDto.ServiceName = businessService.Name;
        appointmentBusinessServiceDto.ServiceType = businessService.ServiceType;
        appointmentBusinessServiceDto.ServicePrice = businessService.Price;
        appointmentBusinessServiceDto.BusinessService = businessService;
        appointmentBusinessServiceDto.Appointment = appointment;
        
        var appointmentBusinessService = _mapper.Map<AppointmentBusinessService>(appointmentBusinessServiceDto);
        await _unitOfWork.AppointmentBusinessServiceRepository.AddAsync(appointmentBusinessService);
        
        //Create meeting
        Meeting meeting = new Meeting
        {
            Date = appointmentBusinessService.Appointment.Date,
            AppointmentBusinessService = appointmentBusinessService,
            Status = MeetingStatus.Undone,
        };
        await _unitOfWork.MeetingRepository.AddAsync(meeting);
        
        //save change
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<CustomerBookingAppointmentResponse>(appointment);
        result.UserAccountPhone = userAccount.PhoneNumber;
        return new BaseResponse<CustomerBookingAppointmentResponse>("User booking appointment successfully", StatusCodeEnum.Created_201, result);
    }

    
    public async Task<BaseResponse<StaffBookingAppointmentResponse>> StaffBookingAppointmentForUser(int staffId,
        StaffBookingAppointmentForCustomerRequest request)
    {
        Slot slot = await _unitOfWork.SlotRepository.GetByIdAsync(request.SlotId);
        if(slot == null) throw new CoreException("Slot not found", StatusCodeEnum.BadRequest_400);
        DateTime date = new DateTime(request.Date.Year, request.Date.Month, request.Date.Day, slot.StartAt.Hours, slot.StartAt.Minutes, slot.StartAt.Seconds);
        User userAccount = await _unitOfWork.UserRepository.GetByIdAsync(request.UserAccountId);
        
        if(userAccount == null) throw new CoreException("User Account not found", StatusCodeEnum.BadRequest_400);
        UserProfile patient = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.PatientId);
        User dentist = await _unitOfWork.DentistRepository.GetByIdAsync(request.DentistId);
        User staff = await _unitOfWork.StaffRepository.GetByIdAsync(staffId);
        
        IEnumerable<Slot> dentistAvailableSlot = await _unitOfWork.SlotRepository.CheckAvailableSlot(dentist.Id, date);
        
        //Validate if dentist free at this slot
        if (!dentistAvailableSlot.Any(a => a.Id == slot.Id)) throw new CoreException("This slot is unavailable", StatusCodeEnum.BadRequest_400);
        BusinessService businessService = await _unitOfWork.ServiceRepository.GetByIdAsync(request.ServiceId);
        //Add user into list of users
        ICollection<User> users = new List<User>();
        users.Add(userAccount);
        users.Add(dentist);
        AppointmentDto appointmentDto = new AppointmentDto
        {
            Date = date,
            UserTreatmentId = patient.Id,
            UserTreatmentName = patient.FirstName + patient.LastName,
            UserAccountId = userAccount.Id,
            UserAccountName = userAccount.FirstName + userAccount.LastName,
            Slot = slot,
            Users = users,
            StaffAccountId = staff.Id,
            StaffAccountName = staff.FirstName + staff.LastName,
        };
        var appointment = _mapper.Map<Appointment>(appointmentDto);
        
        //Set default status for appointment
        appointment.Status = AppointmentStatus.Pending;
        
        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
        
        var appointmentBusinessServiceDto = _mapper.Map<AppointmentBusinessServiceDto>(appointment);
        appointmentBusinessServiceDto.Status = AppointmentBusinessServiceStatus.Undone;
        appointmentBusinessServiceDto.DentistId = dentist.Id;
        appointmentBusinessServiceDto.DentistName = dentist.FirstName + dentist.LastName;
        appointmentBusinessServiceDto.ServiceName = businessService.Name;
        appointmentBusinessServiceDto.ServiceType = businessService.ServiceType;
        appointmentBusinessServiceDto.ServicePrice = businessService.Price;
        appointmentBusinessServiceDto.BusinessService = businessService;
        appointmentBusinessServiceDto.Appointment = appointment;
        
        var appointmentBusinessService = _mapper.Map<AppointmentBusinessService>(appointmentBusinessServiceDto);
        await _unitOfWork.AppointmentBusinessServiceRepository.AddAsync(appointmentBusinessService);
        
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<StaffBookingAppointmentResponse>(appointment);
        result.UserAccountPhone = userAccount.PhoneNumber;
        return new BaseResponse<StaffBookingAppointmentResponse>("User booking appointment successfully", StatusCodeEnum.Created_201, result);
    }
    public async Task<BaseResponse<UpdateAppointmentResponse>> UpdateAppointment(int id, UpdateAppointmentRequest request)
    {
        Appointment appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        _mapper.Map(request, appointment);
        await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<UpdateAppointmentResponse>(appointment);
        return new BaseResponse<UpdateAppointmentResponse>("Update appointment successfully", StatusCodeEnum.OK_200, result);
    }
    public async Task<BaseResponse<DeleteAppointmentResponse>> DeleteAppointment(int id)
    {
        Appointment appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        await _unitOfWork.AppointmentRepository.DeleteAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<DeleteAppointmentResponse>(appointment);
        return new BaseResponse<DeleteAppointmentResponse>("Delete appointment successfully", StatusCodeEnum.OK_200, result);
    }
    
    public async Task<PaginationResponse<UserGetAppointmentResponse>> GetAppointmentByUserId(int userId, int pageNumber, int pageSize)
    {
        var appointments = await _unitOfWork.AppointmentRepository.GetAppointmentByUserId(userId, pageNumber, pageSize);
        int count = await _unitOfWork.AppointmentRepository.CountUserAppointment(userId);
        var result = _mapper.Map<IList<UserGetAppointmentResponse>>(appointments);
        return new PaginationResponse<UserGetAppointmentResponse>(
            "Get all appointments successfully",
            StatusCodeEnum.OK_200,
            result,
            pageNumber,
            pageSize,
            count
        );
    }

    public async Task<BaseResponse<StaffUpdateAppointmentStatusResponse>> StaffUpdateAppointmentStatus(int appointmentId, AppointmentStatus appointmentStatus)
    {
        Appointment appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
        // if (appointment.Status != AppointmentStatus.Scheduled) throw new Exception("Can not perform this action !");
        if (appointmentStatus == AppointmentStatus.Rejected || appointmentStatus == AppointmentStatus.OnGoing || appointmentStatus == AppointmentStatus.Scheduled) 
        appointment.Status = appointmentStatus;
        else throw new Exception("Can not perform this action !");
        await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<StaffUpdateAppointmentStatusResponse>(appointment);
        return new BaseResponse<StaffUpdateAppointmentStatusResponse>("Staff update customer appointment successfully", StatusCodeEnum.OK_200, result);
    }

    public async Task UpdateCurrentAppointmentStatusToDone(int currentAppointmentId)
    {
        Appointment currentAppointment =
            await _unitOfWork.AppointmentRepository.GetAppointmentById(currentAppointmentId);
        if(currentAppointment == null) throw new CoreException("Appointment not found", StatusCodeEnum.BadRequest_400);
        currentAppointment.Status = AppointmentStatus.Done;
        await _unitOfWork.AppointmentRepository.UpdateAsync(currentAppointment);
        await _unitOfWork.SaveChangesAsync();
    }
    public async Task<PaginationResponse<StaffGetAppointmentByDayResponse>> StaffGetAllAppointmentByDay(int pageNumber, int pageSize, DateOnly date)
    {
        
        var appointments = await _unitOfWork.AppointmentRepository.GetAppointmentByDatePagination(pageNumber, pageSize, date);
        int count = await _unitOfWork.AppointmentRepository.CountWhenStaffGetAppointmentByDate(date);
        var result = _mapper.Map<IList<StaffGetAppointmentByDayResponse>>(appointments);
        return new PaginationResponse<StaffGetAppointmentByDayResponse>(
            "Get all appointments successfully",
            StatusCodeEnum.OK_200,
            result,
            pageNumber,
            pageSize,
            count
        );
    }

    public async Task<PaginationResponse<DentistGetTodayAppointmentsResponse>> DentistGetAppointmentByDay(int pageNumber,
        int pageSize, int dentistId, DateOnly date)
    {
        var appointments = await _unitOfWork.AppointmentRepository.DentistGetTodayAppointment(pageNumber, pageSize, dentistId, date);
        int count = await _unitOfWork.AppointmentRepository.CountDentistAppointment(dentistId, date);
        var result = _mapper.Map<IList<DentistGetTodayAppointmentsResponse>>(appointments);
        return new PaginationResponse<DentistGetTodayAppointmentsResponse>(
            "Dentist get all today appointments successfully",
            StatusCodeEnum.OK_200,
            result,
            pageNumber,
            pageSize,
            count
        );
    }
    
    /*
    public async Task<BaseResponse<>
*/
}