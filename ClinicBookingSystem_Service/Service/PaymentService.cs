using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Payment;
using ClinicBookingSystem_Service.Models.Response.Payment;
using ClinicBookingSystem_Service.Models.Response.Transaction;
using ClinicBookingSystem_Service.Models.Utils;
using ClinicBookingSystem_Service.Scheduler;
using ClinicBookingSystem_Service.SignalR.SignalRHub;
using ClinicBookingSystem_Service.ThirdParties.VnPay;
using ClinicBookingSystem_Service.ThirdParties.VnPay.Model.Request;
using Microsoft.AspNetCore.SignalR;
using Quartz;

namespace ClinicBookingSystem_Service.Service;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVnPayService _vnPayService;
    private readonly IMapper _mapper;
    private readonly ISchedulerFactory _schedulerFactory;
    public PaymentService(IUnitOfWork unitOfWork, IVnPayService vnPayService, 
        IMapper mapper, ISchedulerFactory schedulerFactory)
    {
        _unitOfWork = unitOfWork;
        _vnPayService = vnPayService;
        _mapper = mapper;
        _schedulerFactory = schedulerFactory;
    }

    public async Task<BaseResponse<CreateTransactionResponse>> CreateTransaction(
        CreatePaymentTransactionRequest request)
    {
        var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(request.AppointmentId);
        if (appointment == null) throw new CoreException("Appointment not found", StatusCodeEnum.BadRequest_400);

        /*IEnumerable<AppointmentBusinessService> appointmentBusinessServices =
            await _unitOfWork.AppointmentBusinessServiceRepository
                .GetAppointmentBusinessServiceByAppointmentId(appointmentId);

        if(!appointmentBusinessServices.Any()) throw new CoreException("Appointment business service not found", StatusCodeEnum.BadRequest_400);
        */
        List<AppointmentBusinessService>? appointmentBusinessServices = request.AppointmentBusinessServices;
        foreach (var abs in appointmentBusinessServices)
        {
            AppointmentBusinessService availableAbs =
                await _unitOfWork.AppointmentBusinessServiceRepository.GetByIdAsync(abs.Id);
            if (availableAbs == null)
                throw new CoreException("Appointment business service not found", StatusCodeEnum.BadRequest_400);
        }

        IEnumerable<Transaction> transactions = await
            _unitOfWork.TransactionRepository.GetListTransactionByAppointmentId(request.AppointmentId);
        //check if the previous transaction is paid or not
        /*if (transactions.Any(trans => trans.IsPay == false))
        {
            throw new CoreException("Please pay the previous transaction first", StatusCodeEnum.Conflict_409);
        }*/

        var transaction = new Transaction
        {
            Appointment = appointment,
            IsPay = false,
            Status = TransactionStatus.Pending,
        };

        //Create transaction if this is the first time booking (only pay for booking fee)
        if (request.Type == TransactionType.BookingType)
        {
            transaction.Type = TransactionType.BookingType;
            transaction.Amount = appointmentBusinessServices
                .FirstOrDefault(p => p.BusinessService?.IsPreBooking == true)?.ServicePrice;
        }
        //Create transaction if this is the second time booking (pay for service fee)
        else if (request.Type == TransactionType.ServiceType)
        {
            transaction.Type = TransactionType.ServiceType;
            long totalPrice = 0;
            //Calculate total price of all services
            foreach (var abs in appointmentBusinessServices)
            {
                totalPrice += abs.ServicePrice;
            }

            transaction.Amount = totalPrice;
        }

        //Add transaction to database
        await _unitOfWork.TransactionRepository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        
        //set payment time out
        await SetPaymentTimeOut(transaction.Id);
        
        var result = _mapper.Map<CreateTransactionResponse>(transaction);
        var userAccount = await _unitOfWork.UserRepository.GetByIdAsync(appointment.UserAccountId.Value);
        result.UserAccountName = userAccount.FirstName + " " + userAccount.LastName;
        result.UserAccountPhone = userAccount.PhoneNumber;
        return new BaseResponse<CreateTransactionResponse>("Create transaction successfully", StatusCodeEnum.OK_200,
            result);
    }

    public async Task<BaseResponse<CreatePaymentResponse>> CreateVnPayPaymentUrl(CreatePaymentRequest request)
    {
        //nhan vo appointment => list ra appointmentservice => tinh tong tien cua cac appointmentservice
        var transactions =
            await _unitOfWork.TransactionRepository.GetListTransactionByAppointmentId(request.AppointmentId);
        foreach (var transaction in transactions)
        {
            if (transaction.Status == TransactionStatus.Paid)
                throw new CoreException("Appointment has been paid", StatusCodeEnum.BadRequest_400);
        }

        /*var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(appointmentId);
        if(appointment == null) throw new CoreException("Appointment not found", StatusCodeEnum.BadRequest_400);
        var userAccount = await _unitOfWork.UserRepository.GetByIdAsync(appointment.UserAccountId.Value);
        if (userAccount == null) throw new CoreException("User not found", StatusCodeEnum.BadRequest_400);*/

        /*var appointmentBusinessServices =
            await _unitOfWork.AppointmentBusinessServiceRepository
                .GetAppointmentBusinessServiceByAppointmentId(appointmentId);
        long totalPrice = 0;
        foreach (var abs in appointmentBusinessServices)
        {
            totalPrice += abs.ServicePrice;
        }*/
        GenerateTimeStamp generateTimeStamp = new GenerateTimeStamp();
        OrderRequest order = new OrderRequest
        {
            appointmentId = request.AppointmentId,
            OrderId = generateTimeStamp.GetUnixTimeStamp() + request.AppointmentId.ToString(),
            TotalPrice = request.ServicePrice,
        };

        if (request.UserAccountName == null)
            throw new CoreException("UserAccountName is null", StatusCodeEnum.BadRequest_400);
        if (request.UserAccountPhone == null)
            throw new CoreException("UserAccountPhone is null", StatusCodeEnum.BadRequest_400);
        if (request.UserIpAddress == null)
            throw new CoreException("UserIpAddress is null", StatusCodeEnum.BadRequest_400);

        UserInfoRequest userInfo = new UserInfoRequest
        {
            UserIpAddress = request.UserIpAddress,
            UserAccountName = request.UserAccountName,
            UserAccountPhone = request.UserAccountPhone
        };
        string url = _vnPayService.CreatePaymentUrl(order, userInfo);
        return new BaseResponse<CreatePaymentResponse>("Create payment url sucessfully", StatusCodeEnum.OK_200,
            new CreatePaymentResponse
            {
                Url = url
            });
    }

    public async Task<BaseResponse<IEnumerable<SaveVnPayPaymentResponse>>> SaveVnPayPayment(VnPayDataRequest request)
    {
        int appointmentId = int.Parse(request.vnp_OrderInfo);
        var transactions = await _unitOfWork.TransactionRepository.GetListTransactionByAppointmentId(appointmentId);
        
        foreach (var transaction in transactions)
        {
        _mapper.Map(request, transaction);
        }

        var appointment = await _unitOfWork.AppointmentRepository.GetAppointmentById(appointmentId);

        foreach (var transaction in transactions)
        {
            if (transaction.IsPay == false)
            {
                transaction.Appointment = appointment;
                var appointmentBusinessServices =
                    await _unitOfWork.AppointmentBusinessServiceRepository
                        .GetUnPaidAppointmentBusinessServiceByAppointmentId(appointmentId);
                switch (transaction.Type)
                {
                    //Thanh toan phi giu cho, phi kham benh khi dat lich
                    case TransactionType.BookingType:
                        switch (request.vnp_TransactionStatus)
                        {
                            //---------------------------------------------------------------------------//
                            //Thanh toan thanh cong (dat lich kham giu cho)
                            case "00":
                            {
                                //appointment
                                appointment.IsClinicalExamPaid = true;
                                appointment.Status = AppointmentStatus.Scheduled;
                                //appointmentBusinessService
                                foreach (var abs in appointmentBusinessServices)
                                {
                                    abs.IsPaid = true;
                                    abs.TransactionStatus = TransactionStatus.Paid;
                                }

                                //Transaction
                                transaction.Status = TransactionStatus.Paid;
                                transaction.IsPay = true;
                                break;
                            }
                            //---------------------------------------------------------------------------//
                            //---------------------------------------------------------------------------//
                            //Thanh toan that bai do cancel (dat lich kham giu cho)
                            case "02" when request.vnp_ResponseCode == "24":
                            {
                                //appointment
                                appointment.IsClinicalExamPaid = false;
                                appointment.IsActive = false;
                                appointment.Status = AppointmentStatus.Cancelled;
                                //appointmentBusinessService
                                foreach (var abs in appointmentBusinessServices)
                                {
                                    abs.TransactionStatus = TransactionStatus.Cancelled;
                                    abs.IsActive = false;
                                    abs.IsPaid = false;
                                }

                                //Transaction   
                                transaction.Status = TransactionStatus.Cancelled;
                                transaction.IsPay = false;
                                break;
                            }
                            //---------------------------------------------------------------------------//
                            //---------------------------------------------------------------------------//
                            //setup job (chua hoan thien)
                            //Thanh toan that bai do het thoi gian (dat lich kham giu cho)
                            case "02" when request.vnp_ResponseCode == "15":
                            {
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
                            }
                        }

                        //---------------------------------------------------------------------------//
                        break;
                    //Thanh toan phi dich vu 
                    case TransactionType.ServiceType:
                        switch (request.vnp_TransactionStatus)
                        {
                            //---------------------------------------------------------------------------//
                            //Thanh toan thanh cong (dich vu dieu tri)
                            case "00":
                            {
                                //appointment
                                appointment.IsFullyPaid = true;
                                appointment.Status = AppointmentStatus.Waiting;
                                //appointmentBusinessService
                                foreach (var abs in appointmentBusinessServices)
                                {
                                    abs.TransactionStatus = TransactionStatus.Paid;
                                    abs.IsPaid = true;
                                }

                                //Transaction
                                transaction.Status = TransactionStatus.Paid;
                                transaction.IsPay = true;
                                break;
                            }
                            //---------------------------------------------------------------------------//
                            //---------------------------------------------------------------------------//
                            //Thanh toan that bai do cancel (dich vu dieu tri)
                            case "02" when request.vnp_ResponseCode == "24":
                            {
                                //appointment
                                appointment.IsFullyPaid = false;
                                //appointmentBusinessService
                                foreach (var abs in appointmentBusinessServices)
                                {
                                    abs.TransactionStatus = TransactionStatus.Cancelled;
                                    abs.IsPaid = false;
                                }

                                //Transaction
                                transaction.Status = TransactionStatus.Cancelled;
                                transaction.IsActive = false;
                                transaction.IsPay = false;
                                break;
                            }
                            //---------------------------------------------------------------------------//
                            //---------------------------------------------------------------------------//
                            //Thanh toan that bai do het thoi gian (dich vu dieu tri)
                            //setup job (chua hoan thien)
                            case "02" when request.vnp_ResponseCode == "15":
                            {
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
                        }

                        break;
                }

                //---------------------------------------------------------------------------//
                foreach (var abs in appointmentBusinessServices)
                {
                    await _unitOfWork.AppointmentBusinessServiceRepository.UpdateAsync(abs);
                }
                await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                transaction.Amount = transaction.Amount /100;
                await _unitOfWork.TransactionRepository.UpdateAsync(transaction);
            }
        }

        await _unitOfWork.SaveChangesAsync();
        //send to signalr hub
        var result = _mapper.Map<IEnumerable<SaveVnPayPaymentResponse>>(transactions);
        return new BaseResponse<IEnumerable<SaveVnPayPaymentResponse>>("Save payment successfully", StatusCodeEnum.OK_200, result);
    }

      public async Task SetPaymentTimeOut(int transactionId)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        await scheduler.Start();
        
        JobDataMap jobDataMap = new JobDataMap();
        jobDataMap.Put("TransactionId", transactionId);
        IJobDetail job = JobBuilder.Create<PaymentTimeOutJob>()
            .UsingJobData(jobDataMap)
            .Build();
        ITrigger trigger = TriggerBuilder.Create()
            .StartAt(DateTime.Now.AddMinutes(10).AddSeconds(5))
            .Build();
        
        await scheduler.ScheduleJob(job, trigger);
    }
}