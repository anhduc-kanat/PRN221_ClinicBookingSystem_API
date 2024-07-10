﻿using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.RabbitMQ.IService;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/rabbitmq")]
public class RabbitMqController : ControllerBase
{
    private readonly IQueueService _queueService;
    public RabbitMqController(IQueueService queueService)
    {
        _queueService = queueService;
    }
    
    [HttpPost("publish-appointment-to-queue/{appointmentId}")]
    public async Task PublishAppointmentToQueue(int appointmentId)
    {
        await _queueService.PublishAppointmentToQueue(appointmentId);
    }
}