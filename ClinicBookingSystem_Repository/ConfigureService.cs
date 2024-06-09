using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IBaseRepository;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicBookingSystem_Repository;

public static class ConfigureService
{
    public static IServiceCollection ConfigureRepositoryService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBillingRepository, BillingRepository>();
        services.AddScoped<IClaimRepository, ClaimRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IDentistRepository, DentistRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IMedicineRepository, MedicineRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IRelativeRepository, RelativeRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ISalaryRepository, SalaryRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISlotRepository, SlotRepository>();
        services.AddScoped<ISpecificationRepository, SpecificationRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        return services;
    }
}