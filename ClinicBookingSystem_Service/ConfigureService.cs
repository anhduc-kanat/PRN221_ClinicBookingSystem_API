// See https://aka.ms/new-console-template for more information

using System.Configuration;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.IServices;
using ClinicBookingSystem_Service.Mapping;
using ClinicBookingSystem_Service.Models.DTOs.VNPAY;
using ClinicBookingSystem_Service.Models.Utils;
using ClinicBookingSystem_Service.RabbitMQ;
using ClinicBookingSystem_Service.RabbitMQ.Config;
using ClinicBookingSystem_Service.RabbitMQ.Consumers.Appointment;
using ClinicBookingSystem_Service.RabbitMQ.IService;
using ClinicBookingSystem_Service.RabbitMQ.Service;
using ClinicBookingSystem_Service.Scheduler;
using ClinicBookingSystem_Service.Scheduler.BackgroundTask;
using ClinicBookingSystem_Service.Service;
using ClinicBookingSystem_Service.Services;
using ClinicBookingSystem_Service.SignalR.SignalRClient;
using ClinicBookingSystem_Service.SignalR.SignalRHub;
using ClinicBookingSystem_Service.ThirdParties.VnPay;
using global::System;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

public static class ConfigureService
{
    public static IServiceCollection ConfigureServiceService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfiles));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IDentistService, DentistService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<HashPassword>();
        services.AddScoped<GeneratePassword>();
        services.AddScoped<CheckPassword>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenService, AuthenService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<ISpecificationService, SpecificationService>();
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IClinicOwnerService, ClinicOwnerService>();
        services.AddScoped<IVnPayService, VnPayService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<IQueueService, QueueService>();

        //quartz
        services.AddQuartz(p =>
        {
            p.UseMicrosoftDependencyInjectionJobFactory();
            var paymentJobKey = new JobKey("PaymentTimeOutJob");
            p.AddJob<PaymentTimeOutJob>(opt => opt.WithIdentity(paymentJobKey).StoreDurably());
            
            
            var checkMeetingJobKey = new JobKey("CheckMeetingJob");
            p.AddJob<CheckMeetingJob>(opt => opt.WithIdentity(checkMeetingJobKey).StoreDurably());
            p.AddTrigger(t =>
                t.WithIdentity("CheckMeetingJobTrigger")
                    .ForJob(checkMeetingJobKey)
                    .StartNow()
                    .WithDailyTimeIntervalSchedule
                    (s =>
                        s.WithIntervalInHours(24)
                            .OnEveryDay()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(5, 0))
                    ));
            
            services.AddQuartzHostedService(q =>
                q.WaitForJobsToComplete = true);
        });
        
        //RabbitMQ
        var rabbitMQConfigSection = configuration.GetSection("RabbitMQ");
        var rabbitMQConfig = new RabbitMQConfig
        {
            HostName = rabbitMQConfigSection["HostName"],
            UserName = rabbitMQConfigSection["UserName"],
            Password = rabbitMQConfigSection["Password"],
            Port = Convert.ToInt32(rabbitMQConfigSection["Port"])
        };
        
        services.AddScoped<IRabbitMQBus, RabbitMQBus>();
        services.AddSingleton(rabbitMQConfig);
        services.AddSingleton<RabbitMQConnection>();
        services.AddSingleton<IRabbitMQService, RabbitMQService>();
        services.AddMassTransit(config =>
        {
            //consumers
            config.AddConsumer<GetAllConsumer>();
            
            //register rabbitmq
            config.UsingRabbitMq((ctx, cfg) =>
            {
                var rabbitMQConfig = ctx.GetService<RabbitMQConfig>();
                Console.WriteLine($"rabbitmq://${rabbitMQConfig.HostName}:${rabbitMQConfig.Port}");
                cfg.Host(new Uri($"rabbitmq://{rabbitMQConfig.HostName}:{rabbitMQConfig.Port}"), h =>
                {
                    h.Username(rabbitMQConfig.UserName);
                    h.Password(rabbitMQConfig.Password);
                });
                cfg.ConfigureEndpoints(ctx);
                cfg.UseRawJsonSerializer();
            });

            
        });
        services.AddMassTransitHostedService();
        //signalR
        services.AddSignalR();
        services.AddSingleton<AppointmentHub>();
        //signalR client
        services.AddSingleton<AppointmentClient>(provider =>
        {
            var rabbitMqService = provider.GetRequiredService<IRabbitMQService>();
            var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            var hubUrl = "https://localhost:7002/appointmentHub";
            return new AppointmentClient(hubUrl, rabbitMqService, unitOfWork);
        });
        
        services.AddHostedService<CheckAppointmentBackgroundService>();
        return services;
    }
}