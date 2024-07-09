﻿using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.RabbitMQ.IService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ClinicBookingSystem_Service.Scheduler.BackgroundTask;

public class CheckAppointmentBackgroundService : BackgroundService
{
    private readonly ILogger<CheckAppointmentBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IRabbitMQService _rabbitMqService;
    public CheckAppointmentBackgroundService(ILogger<CheckAppointmentBackgroundService> logger,
        IServiceProvider serviceProvider,
        IRabbitMQService rabbitMqService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _rabbitMqService = rabbitMqService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var provider = _serviceProvider.CreateScope();
            var unitOfWork = provider.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _logger.LogInformation("5 secs job: CheckAppointmentBackgroundService is starting.");
        
        
            /*var appointments = await unitOfWork.AppointmentRepository.GetTodayMeetingTreatmentAppointment();
            if(appointments.IsNullOrEmpty()) return;
            foreach (var appointment in appointments)
            {

            }*/
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
        
    }

    /*private async Task CheckMeetingStatus(IUnitOfWork unitOfWork)
    {
        _logger.LogInformation("Check MeetingStatus is starting.");
        _logger.LogInformation($"{DateTime.Now}");
        var meetings = await unitOfWork.MeetingRepository.GetMeetingByToday(DateTime.Now);
        if (meetings.IsNullOrEmpty()) return;
        foreach (var meeting in meetings)
        {
            _logger.LogInformation($"{meeting.Id} - {meeting.Status}");
            if (meeting.Status != MeetingStatus.Future) continue;
            meeting.Status = MeetingStatus.Waiting;
            await unitOfWork.MeetingRepository.UpdateAsync(meeting);
        }
        await unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Check MeetingStatus has been done.");
    }*/
}