using ClinicBookingSystem_Repository.IBaseRepository;

namespace ClinicBookingSystem_Repository.IRepositories;

public interface IUnitOfWork : IBaseUnitOfWork
{
    IUserRepository UserRepository { get; }
    IApplicationRepository ApplicationRepository { get; }
    IDentistRepository DentistRepository { get; }
    IRoleRepository RoleRepository { get; }
    IStaffRepository StaffRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    ITokenRepository TokenRepository { get; }
    ISlotRepository SlotRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    IBillingRepository BillingRepository { get; }
    IClaimRepository ClaimRepository { get; }
    IClinicRepository ClinicRepository { get; }
    IMedicalRecordRepository MedicalRecordRepository { get; }
    IMedicineRepository MedicineRepository { get; }
    IUserProfileRepository UserProfileRepository { get; }
    IResultRepository ResultRepository { get; }
    IServiceRepository ServiceRepository { get; }
    ISpecificationRepository SpecificationRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    ISalaryRepository SalaryRepository { get; }
    IClinicOwnerRepository ClinicOwnerRepository { get; }
    IAppointmentBusinessServiceRepository AppointmentBusinessServiceRepository { get; }
    IMeetingRepository MeetingRepository { get; }
}