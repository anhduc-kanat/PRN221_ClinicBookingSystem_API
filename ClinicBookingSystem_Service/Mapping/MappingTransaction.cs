using System.Globalization;
using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Request.Transaction;
using ClinicBookingSystem_Service.Models.Response.Payment;
using ClinicBookingSystem_Service.Models.Response.Transaction;
using ClinicBookingSystem_Service.ThirdParties.VnPay.Model.Request;
using MassTransit.Configuration;

namespace ClinicBookingSystem_Service.Mapping;

public class MappingTransaction : Profile
{
    public MappingTransaction()
    {
        CreateMap<Transaction, GetTransactionResponse>()
            .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => src.Appointment));
        
        CreateMap<CreateTransactionRequest, Transaction>();
        CreateMap<UpdateTransactionRequest, Transaction>();
        CreateMap<Transaction, CreateTransactionResponse>().ReverseMap();
        CreateMap<Transaction, UpdateTransactionResponse>().ReverseMap();
        CreateMap<Transaction, DeleteTransactionResponse>().ReverseMap();

        CreateMap<VnPayDataRequest, Transaction>()
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.vnp_Amount))
            .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.vnp_BankCode))
            .ForMember(dest => dest.BankTranNo, opt => opt.MapFrom(src => src.vnp_BankTranNo))
            .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.vnp_CardType))
            .ForMember(dest => dest.OrderInfo, opt => opt.MapFrom(src => src.vnp_OrderInfo))
            .ForMember(dest => dest.PayDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.ResponseCode, opt => opt.MapFrom(src => src.vnp_ResponseCode))
            .ForMember(dest => dest.TransactionNo, opt => opt.MapFrom(src => src.vnp_TransactionNo))
            .ForMember(dest => dest.TransactionStatus, opt => opt.MapFrom(src => src.vnp_TransactionStatus))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.vnp_TxnRef));
        CreateMap<Transaction, SaveVnPayPaymentResponse>().ReverseMap();
        CreateMap<CreateTransactionResponse, Transaction>().ReverseMap()
            .ForMember(dest => dest.ServicePrice, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Appointment.Id));
        
    }
}