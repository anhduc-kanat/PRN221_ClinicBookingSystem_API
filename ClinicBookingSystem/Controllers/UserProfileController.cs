﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Relative;
using ClinicBookingSystem_Service.Models.Request.UserProfile;
using ClinicBookingSystem_Service.Models.Response.UserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        //
        [HttpPost]
        [Route("new")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult<BaseResponse<CreateUserProfileResponse>>> Create( [FromBody] CreateUserProfileRequest request)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
            return await _userProfileService.AddUserProfile(userId, request);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<BaseResponse<UpdateUserProfileResponse>>> Update(int id, [FromBody] UpdateUserProfileRequest request)
        {
            return await _userProfileService.UpdateUserProfile(id, request);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<BaseResponse<DeleteUserProfileResponse>>> Delete(int id)
        {
            return await _userProfileService.DeleteUserProfile(id);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<BaseResponse<GetUserProfileResponse>>> GetById(int id)
        {
            var user = await _userProfileService.GetUserProfile(id);
            return user;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetUserProfileResponse>>>> GetAll()
        {
            return await _userProfileService.GetUserProfiles();

        }

        [HttpGet]
        [Route("all-by-user")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetUserProfileResponse>>>> GetUserProfilesByUser(string phone)
        {
            return await _userProfileService.GetUserProfilesByUser(phone);
        }
        
        /// <summary>
        /// Lấy tất cả các profile của customer, login vào và lấy Bearer token truyền vô headers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-profile-by-customer")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetUserProfileResponse>>>> GetUserProfileByUserId()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
            return await _userProfileService.GetUserProfileByUserId(userId);
        }
        
        [HttpGet]
        [Route("get-profile-by-user-account-id")]
        [Authorize(Roles = "STAFF, DENTIST")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetUserProfileResponse>>>> GetUserProfileByInputUserAccountId(int userId)
        {
            return await _userProfileService.GetUserProfileByUserAccountId(userId);
        }
    }
}