using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicBookingSystem_DataAccessObject;

public static class ConfigureService
{
    public static IServiceCollection ConfigureDataAccessObjectService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<UserDAO>();
        services.AddScoped<ApplicationDAO>();
        services.AddScoped<RoleDAO>();
        services.AddScoped<DentistDAO>();
        services.AddScoped<StaffDAO>();
        services.AddScoped<CustomerDAO>();
        services.AddScoped<RoleDAO>();
        services.AddScoped<TokenDAO>();
        services.AddScoped<SlotDAO>();
        services.AddScoped<AppointmentDAO>();
        services.AddScoped<ClaimDAO>();
        services.AddScoped<BillingDAO>();
        services.AddScoped<ClinicDAO>();
        services.AddScoped<MedicalRecordDAO>();
        services.AddScoped<MedicineDAO>();
        services.AddScoped<UserProfileDAO>();
        services.AddScoped<ResultDAO>();
        services.AddScoped<SalaryDAO>();
        services.AddScoped<SpecificationDAO>();
        services.AddScoped<TransactionDAO>();
        services.AddScoped<ServiceDAO>();
        services.AddScoped<ClinicOwnerDAO>();
        services.AddScoped<AppointmentBusinessServiceDAO>();
        services.AddScoped<MeetingDAO>();
        services.AddScoped(typeof(IBaseDAO<>), typeof(BaseDAO<>));

        return services;
    }
}