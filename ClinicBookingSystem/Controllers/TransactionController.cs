﻿using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Pagination;
using ClinicBookingSystem_Service.Models.Request.Transaction;
using ClinicBookingSystem_Service.Models.Response.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    //get all transactions
    [HttpGet]
    [Route("get-all-transaction")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetTransactionResponse>>>> GetTransactions()
    {
        return Ok(await _transactionService.GetAllTransaction());
    }
    //get transaction by id
    [HttpGet]
    [Route("get-transaction-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetTransactionResponse>>> GetTransactionById(int id)
    {
        return Ok(await _transactionService.GetTransactionById(id));
    }
    //create transaction
    [HttpPost]
    [Route("create-transaction")]
    public async Task<ActionResult<BaseResponse<CreateTransactionResponse>>> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        return Ok(await _transactionService.CreateTransaction(request));
    }
    //update transaction
    [HttpPut]
    [Route("update-transaction/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateTransactionResponse>>> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest request)
    {
        return Ok(await _transactionService.UpdateTransaction(id, request));
    }
    //delete transaction
    [HttpDelete]
    [Route("delete-transaction/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteTransactionResponse>>> DeleteTransaction(int id)
    {
        return Ok(await _transactionService.DeleteTransaction(id));
    }

    [HttpGet]
    [Route("get-transaction-user/{id}")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetTransactionResponse>>>> GetTransactionsByUserId(int id)
    {
        return Ok(await _transactionService.GetAllTransactionByUserId(id));
    }

}