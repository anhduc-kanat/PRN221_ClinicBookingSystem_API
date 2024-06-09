﻿// See https://aka.ms/new-console-template for more information

using ClinicBookingSystem_Service;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.IServices;
using ClinicBookingSystem_Service.Mapping;
using ClinicBookingSystem_Service.Models.Utils;
using ClinicBookingSystem_Service.Service;
using ClinicBookingSystem_Service.Services;
using global::System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigureService
{
    public static IServiceCollection ConfigureServiceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfiles));
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IDentistService, DentistService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<HashPassword>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenService, AuthenService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IMedicalRecordService, MedicalRecordService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IMedicineService, MedicineService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<ISpecificationService, SpecificationService>();
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<IClinicOwnerService, ClinicOwnerService>();
        return services;
    }
}

