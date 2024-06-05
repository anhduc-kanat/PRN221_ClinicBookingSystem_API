﻿// See https://aka.ms/new-console-template for more information

using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.IServices;
using ClinicBookingSystem_Service.Mapping;
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
        services.AddAutoMapper(typeof(MappingDentist));
        services.AddAutoMapper(typeof(MappingStaff));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IDentistService, DentistService>();
        services.AddScoped<IStaffService, StaffService>();
        return services;
    }
}

