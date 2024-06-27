﻿using System.Net;
using System.Net.Sockets;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Payment;
using ClinicBookingSystem_Service.Models.Response.Payment;
using ClinicBookingSystem_Service.ThirdParties.VnPay.Model.Request;
using ClinicBookingSystem_Service.ThirdParties.VnPay.Model.Response;
using ClinicBookingSystem.Common;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;
[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly GetUserIpAddress _getUserIpAddress;
    
    public PaymentController(IPaymentService paymentService,
        GetUserIpAddress getUserIpAddress)
    {
        _paymentService = paymentService;
        _getUserIpAddress = getUserIpAddress;
    }
    
    [HttpGet]
    [Route("get-vnpay-payment-url")]
    public async Task<BaseResponse<CreatePaymentResponse>> GetVnPayUrl(int appointmentId)
    {
        string ipAddress = _getUserIpAddress.GetIpAddress();
        UserInfoRequest userInfoRequest = new UserInfoRequest
        {
            UserIpAddress = ipAddress
        };
        return await _paymentService.CreateVnPayPaymentUrl(appointmentId, userInfoRequest);
    }

    [HttpGet]
    [Route("save-payment")]
    public async Task<BaseResponse<SaveVnPayPaymentResponse>> SavePayment([FromQuery] VnPayDataRequest request)
    {
        var query = HttpContext.Request.Query;
        
        request = new VnPayDataRequest
        {
            vnp_Amount = long.Parse(query["vnp_Amount"]),
            vnp_BankCode = query["vnp_BankCode"],
            vnp_BankTranNo = query["vnp_BankTranNo"],
            vnp_CardType = query["vnp_CardType"],
            vnp_OrderInfo = query["vnp_OrderInfo"],
            vnp_PayDate = query["vnp_PayDate"],
            vnp_ResponseCode = query["vnp_ResponseCode"],
            vnp_TransactionNo = query["vnp_TransactionNo"],
            vnp_TransactionStatus = query["vnp_TransactionStatus"],
            vnp_TxnRef = query["vnp_TxnRef"]
        };
        return await _paymentService.SaveVnPayPayment(request);
    }
}