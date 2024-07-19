﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Transaction;
using ClinicBookingSystem_Service.Models.Response.Transaction;
using Org.BouncyCastle.Pqc.Crypto.Lms;
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
        Transaction transaction = await _unitOfWork.TransactionRepository.GetTransactionByTransactionId(id);
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
        IEnumerable<Transaction> transactions = await _unitOfWork.TransactionRepository.GetAllTransaction();
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
<<<<<<< HEAD

    public async Task<BaseResponse<List<Dictionary<int, long?>>>> GetStatisticOfTransaction()
    {
        // Lấy danh sách các giao dịch từ cơ sở dữ liệu
        IEnumerable<Transaction> transactions = await _unitOfWork.TransactionRepository.GetAllAsync();

        // Tạo từ điển để lưu số tiền tổng hợp theo từng tháng
        Dictionary<int, long?> monthTotals = new Dictionary<int, long?>();

        // Duyệt qua từng giao dịch
        foreach (var transaction in transactions)
        {
            // Lấy tháng từ PayDate
            int month = transaction.PayDate?.Month ?? 0;

            // Kiểm tra xem tháng đã tồn tại trong từ điển chưa
            if (month != 0)
            {
                if (monthTotals.ContainsKey(month))
                {
                    // Cộng thêm số tiền vào giá trị hiện tại của tháng
                    monthTotals[month] += transaction.Amount;
                }
                else
                {
                    // Thêm tháng mới vào từ điển với số tiền hiện tại
                    monthTotals[month] = transaction.Amount;
                }
            }
        }

        // Tạo danh sách các từ điển
        var result = new List<Dictionary<int, long?>>();
        foreach (var month in monthTotals)
        {
            var monthDictionary = new Dictionary<int, long?>
        {
            { month.Key, month.Value }
        };
            result.Add(monthDictionary);
        }

        // Trả về kết quả
        return new BaseResponse<List<Dictionary<int, long?>>>("Get all transaction successfully", StatusCodeEnum.OK_200, result);
    }

=======
    
    public async Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransactionByDate(DateOnly dateOnly)
    {
        DateTime date = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
        IEnumerable<Transaction> transactions = await _unitOfWork.TransactionRepository.GetTransactionByDate(date);
        var result = _mapper.Map< IEnumerable <GetTransactionResponse>>(transactions);
        return new BaseResponse<IEnumerable<GetTransactionResponse>>("Get all transaction successfully", StatusCodeEnum.OK_200, result);
    }
>>>>>>> 4b3a626216b7193f60aeaaa5044b2bf850cddae1
}