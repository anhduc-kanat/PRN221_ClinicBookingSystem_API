using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.RabbitMQ.IService;

namespace ClinicBookingSystem_Service.Service;

public class QueueService : IQueueService
{
    private readonly IRabbitMQService _rabbitMqService;
    private readonly IUnitOfWork _unitOfWork;
    public QueueService(IRabbitMQService rabbitMqService,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task PublishAppointmentToQueue(int meetingId, int dentistId)
    {
        var meeting = await _unitOfWork.MeetingRepository.GetTreatmentMeetingQueueByMeetingId(meetingId, DateTime.Now);
        if(meeting == null)
            throw new CoreException("There is no AppointmentBusinessService suitable", StatusCodeEnum.BadRequest_400);
        
        var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(meeting.AppointmentBusinessService.Appointment.Id);
        if (appointment == null) throw new CoreException("Appointment not found", StatusCodeEnum.BadRequest_400);
        if(appointment.Status != AppointmentStatus.Waiting) 
            throw new CoreException("Appointment is not in waiting for queue", StatusCodeEnum.BadRequest_400);
        
        /*
        var appointmentBusinessService = appointment.AppointmentBusinessServices.FirstOrDefault(p =>
            p.Meetings.Any(p => p.Date.Value.Year == DateTime.Now.Year &&
                                 p.Date.Value.Month == DateTime.Now.Month &&
                                 p.Date.Value.Day == DateTime.Now.Day &&
                                 p.Status == MeetingStatus.CheckIn) && p.ServiceType == ServiceType.Treatment);*/

        
        var dentist = await _unitOfWork.DentistRepository.GetDentistById(dentistId);
        if(dentist == null) throw new CoreException("Dentist not found", StatusCodeEnum.BadRequest_400);
        /*if(!dentist.BusinessServices.Any(p => p.Id == meeting.AppointmentBusinessService.BusinessService.Id))
            throw new CoreException("Dentist not provide this service", StatusCodeEnum.BadRequest_400);*/
        if(!dentist.Specifications.Any(p => p.BusinessServices.Any(p => p.Id == meeting.AppointmentBusinessService.BusinessService.Id)))
            throw new CoreException("Dentist not provide this service", StatusCodeEnum.BadRequest_400);
        _rabbitMqService.PublishMessage(dentist.PhoneNumber, meeting.Id.ToString());
        meeting.Status = MeetingStatus.InQueue;
        appointment.Status = AppointmentStatus.OnTreatment;
        
        await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.MeetingRepository.UpdateAsync(meeting);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ConsumeMessageDentistQueue(string dentistPhoneNumber)
    {
        var meetingId = _rabbitMqService.ConsumeMessage(dentistPhoneNumber);
        
        var meeting = await _unitOfWork.MeetingRepository.GetMeetingById(int.Parse(meetingId));
        if(meeting.Date.Value.Year != DateTime.Now.Year ||
           meeting.Date.Value.Month != DateTime.Now.Month ||
           meeting.Date.Value.Day != DateTime.Now.Day) 
            throw new CoreException("Meeting is not today", StatusCodeEnum.BadRequest_400);
        
        var dentist = await _unitOfWork.DentistRepository.GetDentistById((int)meeting.DentistId);
        
        meeting.Status = MeetingStatus.InTreatment;
        dentist.IsBusy = true;
        
        await _unitOfWork.DentistRepository.UpdateAsync(dentist);
        await _unitOfWork.MeetingRepository.UpdateAsync(meeting);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task<int> GetQueueLength(string dentistPhoneNumber)
    {
        return _rabbitMqService.GetQueueLength(dentistPhoneNumber);
    }
}