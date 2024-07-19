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
<<<<<<< HEAD

    Task<BaseResponse<List< Dictionary<int, long?>>>> GetStatisticOfTransaction();
=======
    Task<BaseResponse<IEnumerable<GetTransactionResponse>>> GetAllTransactionByDate(DateOnly dateOnly);

>>>>>>> 4b3a626216b7193f60aeaaa5044b2bf850cddae1
}