using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Transaction;
using ClinicBookingSystem_Service.Models.Response.Transaction;

namespace ClinicBookingSystem_Service.IService;

public interface ITransactionService
{
    //Create transaction
    Task<BaseResponse<CreateTransactionResponse>> CreateTransaction(CreateTransactionRequest request);
    //get transaction by id
    Task<BaseResponse<GetTransactionResponse>> GetTransactionById(int id);
    //update transaction
    Task<BaseResponse<UpdateTransactionResponse>> UpdateTransaction(int id, UpdateTransactionRequest request);
    //delete transaction
    Task<BaseResponse<DeleteTransactionResponse>> DeleteTransaction(int id);
    //get all transactions
    Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransaction();
    Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransactionByUserId(int userId);

    Task<BaseResponse<List< Dictionary<int, long?>>>> GetStatisticOfTransaction();
    Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransactionByDate(DateOnly dateOnly);

}