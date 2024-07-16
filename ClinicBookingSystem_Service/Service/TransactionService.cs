﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Transaction;
using ClinicBookingSystem_Service.Models.Response.Transaction;
using System.Collections.Generic;

namespace ClinicBookingSystem_Service.Service;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    //Create transaction
    public async Task<BaseResponse<CreateTransactionResponse>> CreateTransaction(CreateTransactionRequest request)
    {
        Transaction transaction = _mapper.Map<Transaction>(request);
        await _unitOfWork.TransactionRepository.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<CreateTransactionResponse>(transaction);
        return new BaseResponse<CreateTransactionResponse>("Create transaction successfully",
            StatusCodeEnum.Created_201, result);
    }
    //get transaction by id
    public async Task<BaseResponse<GetTransactionResponse>> GetTransactionById(int id)
    {
        Transaction transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
        var result = _mapper.Map<GetTransactionResponse>(transaction);
        return new BaseResponse<GetTransactionResponse>("Get transaction by id successfully", StatusCodeEnum.OK_200, result);
    }
    //update transaction
    public async Task<BaseResponse<UpdateTransactionResponse>> UpdateTransaction(int id, UpdateTransactionRequest request)
    {
        Transaction transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
        _mapper.Map(request, transaction);
        _unitOfWork.TransactionRepository.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<UpdateTransactionResponse>(transaction);
        return new BaseResponse<UpdateTransactionResponse>("Update transaction successfully", StatusCodeEnum.OK_200,
            result);
    }
    //delete transaction
    public async Task<BaseResponse<DeleteTransactionResponse>> DeleteTransaction(int id)
    {
        Transaction transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
        await _unitOfWork.TransactionRepository.DeleteAsync(transaction);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<DeleteTransactionResponse>(transaction);
        return new BaseResponse<DeleteTransactionResponse>("Delete transaction successfully", StatusCodeEnum.OK_200, result);
    }
    //get all transactions
    public async Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransaction()
    {
        IEnumerable<Transaction> transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GetTransactionResponse>>(transactions);
        return new BaseResponse<IEnumerable<GetTransactionResponse>>("Get all transaction sucessfully", StatusCodeEnum.OK_200, result);
    }

    //get all transaction by user id
   

    public async Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransactionByUserId(int userId)
    {
        IEnumerable<Transaction> transactions = await _unitOfWork.TransactionRepository.GetListTransactionByUserId(userId);
        var result = _mapper.Map< IEnumerable <GetTransactionResponse>>(transactions);
        return new BaseResponse<IEnumerable<GetTransactionResponse>>("Get all transaction successfully", StatusCodeEnum.OK_200, result);
    }
}