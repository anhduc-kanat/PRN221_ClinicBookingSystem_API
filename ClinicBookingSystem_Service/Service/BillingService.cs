﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Billing;
using ClinicBookingSystem_Service.Models.Response.Billing;

namespace ClinicBookingSystem_Service.Service;

public class BillingService : IBillingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public BillingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    //Get all billings
    public async Task<BaseResponse<IEnumerable<GetBillingResponse>>> GetAllBilling()
    {
        IEnumerable<Billing> billings = await _unitOfWork.BillingRepository.GetAllAsync();
        return new BaseResponse<IEnumerable<GetBillingResponse>>("Get all billings successfully", StatusCodeEnum.OK_200,
            _mapper.Map<IEnumerable<GetBillingResponse>>(billings));
    }

    //Get billing by id
    public async Task<BaseResponse<GetBillingResponse>> GetBillingById(int id) 
    {
        Billing billing = await _unitOfWork.BillingRepository.GetByIdAsync(id);
        return new BaseResponse<GetBillingResponse>("Get billing by id successfully", StatusCodeEnum.OK_200,
            _mapper.Map<GetBillingResponse>(billing));
    }
    //Create billing
    public async Task<BaseResponse<CreateBillingResponse>> CreateBilling(CreateBillingRequest request)
    {
        var billing = _mapper.Map<Billing>(request);
        await _unitOfWork.BillingRepository.AddAsync(billing);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CreateBillingResponse>("Create billing successfully", StatusCodeEnum.Created_201,
            _mapper.Map<CreateBillingResponse>(billing));
    }
    //Update billing
    public async Task<BaseResponse<UpdateBillingResponse>> UpdateBilling(int id, UpdateBillingRequest request)
    {
        var billing = await _unitOfWork.BillingRepository.GetByIdAsync(id);
        _mapper.Map(request, billing);
        _unitOfWork.BillingRepository.UpdateAsync(billing);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<UpdateBillingResponse>("Update billing successfully", StatusCodeEnum.OK_200,
            _mapper.Map<UpdateBillingResponse>(billing));
    }
    //Delete billing
    public async Task<BaseResponse<DeleteBillingResponse>> DeleteBilling(int id)
    {
        var billing = await _unitOfWork.BillingRepository.GetByIdAsync(id);
        await _unitOfWork.BillingRepository.DeleteAsync(billing);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<DeleteBillingResponse>("Delete billing successfully", StatusCodeEnum.OK_200,
            _mapper.Map<DeleteBillingResponse>(billing));
    }
    
}