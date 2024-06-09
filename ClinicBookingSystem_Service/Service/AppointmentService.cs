using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Appointment;
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
        IEnumerable<Appointment> appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GetAppointmentResponse>>(appointments);
        return new BaseResponse<IEnumerable<GetAppointmentResponse>>("Get all appointments successfully", StatusCodeEnum.OK_200, result);
    }

    public async Task<BaseResponse<GetAppointmentResponse>> GetAppointmentById(int id)
    {
        Appointment appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
        var result = _mapper.Map<GetAppointmentResponse>(appointment);
        return new BaseResponse<GetAppointmentResponse>("Get appointment by id successfully", StatusCodeEnum.OK_200, result);
    }

    public async Task<BaseResponse<CreateAppointmentResponse>> CreateAppointment(CreateAppointmentRequest request)
    {
        Appointment appointment = _mapper.Map<Appointment>(request);
        await _unitOfWork.AppointmentRepository.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<CreateAppointmentResponse>(appointment);
        return new BaseResponse<CreateAppointmentResponse>("Create appointment successfully", StatusCodeEnum.Created_201, result);
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
}