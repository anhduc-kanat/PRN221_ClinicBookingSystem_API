using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Request.Customer;
using ClinicBookingSystem_Service.Models.Request.Relative;
using ClinicBookingSystem_Service.Models.Request.UserProfile;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Response.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.IService
{
    public interface IUserProfileService
    {
        Task<BaseResponse<CreateUserProfileResponse>> AddUserProfile(int userId, CreateUserProfileRequest request);
        Task<BaseResponse<UpdateUserProfileResponse>> UpdateUserProfile(int id, UpdateUserProfileRequest request);
        Task<BaseResponse<DeleteUserProfileResponse>> DeleteUserProfile(int id);
        Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfilesByUser(string phone);

        Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfiles();
        Task<BaseResponse<GetUserProfileResponse>> GetUserProfile(int id);
        Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfileByUserId(int userId);
        Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfileByUserAccountId(int userId);
    }
}