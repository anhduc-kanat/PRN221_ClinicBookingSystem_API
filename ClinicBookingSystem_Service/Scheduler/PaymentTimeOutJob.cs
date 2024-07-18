using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ClinicBookingSystem_Service.Scheduler;

public class PaymentTimeOutJob : IJob
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentTimeOutJob> _logger;

    public PaymentTimeOutJob(IUnitOfWork unitOfWork, ILogger<PaymentTimeOutJob> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("PaymentTimeOutJob is running...");
        JobDataMap jobDataMap = context.JobDetail.JobDataMap;
        int transactionId = jobDataMap.GetInt("TransactionId");
        var transaction = await _unitOfWork.TransactionRepository.GetTransactionByTransactionId(transactionId);
        var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(transaction.Appointment.Id);

        if (transaction.IsPay == false && transaction.Status == TransactionStatus.Pending)
        {
            transaction.Appointment = appointment;
            var appointmentBusinessServices =
                await _unitOfWork.AppointmentBusinessServiceRepository
                    .GetUnPaidAppointmentBusinessServiceByAppointmentId(appointment.Id);
            switch (transaction.Type)
            {
                //Thanh toan phi giu cho, phi kham benh khi dat lich
                case TransactionType.BookingType:
                    //appointment
                    appointment.IsClinicalExamPaid = false;
                    appointment.IsActive = false;
                    appointment.Status = AppointmentStatus.Cancelled;
                    //appointmentBusinessService
                    foreach (var abs in appointmentBusinessServices)
                    {
                        abs.TransactionStatus = TransactionStatus.Overdue;
                        abs.IsActive = false;
                        abs.IsPaid = false;
                    }

                    //Transaction
                    transaction.Status = TransactionStatus.Overdue;
                    transaction.IsPay = false;
                    break;

                //Thanh toan phi dich vu 
                case TransactionType.ServiceType:
                    //Thanh toan that bai do het thoi gian (dich vu dieu tri)
                    //setup job (chua hoan thien)
                    //appointment
                    appointment.IsFullyPaid = false;
                    //appointmentBusinessService
                    foreach (var abs in appointmentBusinessServices)
                    {
                        abs.TransactionStatus = TransactionStatus.Overdue;
                        abs.IsPaid = false;
                    }

                    //Transaction
                    transaction.Status = TransactionStatus.Overdue;
                    transaction.IsActive = false;
                    transaction.IsPay = false;
                    break;
            }

            //---------------------------------------------------------------------------//
            foreach (var abs in appointmentBusinessServices)
            {
                await _unitOfWork.AppointmentBusinessServiceRepository.UpdateAsync(abs);
            }

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
        }
        _logger.LogInformation("PaymentTimeOutJob is done...");
        await _unitOfWork.SaveChangesAsync();
    }
}