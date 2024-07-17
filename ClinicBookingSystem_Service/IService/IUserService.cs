using System.Collections;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.User;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Response.User;

namespace ClinicBookingSystem_Service.IService;

public interface IUserService
{
    Task<BaseResponse<GetMyProfileResponse>> GetMyProfile(int userId);
    
    
    
    
    
    Task<BaseResponse<ChangePasswordResponse>> ChangePassword(int userId, string oldPassword, string newPassword);






















    /*Task<BaseResponse<IEnumerable<GetAllUserResponse>>> GetAllUser();
    Task<BaseResponse<IEnumerable<GetAllUserResponse>>> GetAllUserFromBase();
    Task<BaseResponse<GetUserByIdResponse>> GetUserById(int id);
    Task<BaseResponse<GetUserByIdResponse>> GetUserByIdFromBase(int id);
    Task<BaseResponse<UpdateUserResponse>> UpdateUser(int id, UpdateUserRequest user);
    Task<BaseResponse<UpdateUserResponse>> UpdateUserFromBase(int id, UpdateUserRequest user);
    Task<BaseResponse<DeleteUserResponse>> DeleteUser(int id);
    Task<BaseResponse<DeleteUserResponse>> DeleteUserFromBase(int id);
    Task<BaseResponse<CreateNewUserResponse>> CreateUser(CreateNewUserRequest request);
    Task<BaseResponse<CreateNewUserResponse>> CreateUserFromBase(CreateNewUserRequest request);*/
}