using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Result;
using ClinicBookingSystem_Service.Models.Response.Result;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_Service.IService;

public interface IResultService
{
    Task<BaseResponse<IEnumerable<GetResultResponse>>> GetAllResult();
    Task<BaseResponse<GetResultResponse>> GetResultById(int id);
    Task<BaseResponse<CreateResultResponse>> CreateResult(CreateResultRequest result);
    Task<BaseResponse<UpdateResultResponse>> UpdateResult(int id, UpdateResultRequest result);
    Task<BaseResponse<DeleteResultResponse>> DeleteResult(int id);

}