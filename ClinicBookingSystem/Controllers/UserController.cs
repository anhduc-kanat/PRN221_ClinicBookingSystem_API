﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.User;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Response.User;
using ClinicBookingSystem_Service.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Lấy ra thông tin profile của toàn bộ user trong hệ thống, cần truyền Bearer token vào header
        /// </summary>
        /// <remarks>
        /// - Đối với mọi loại nhân viên (gồm staff và dentist):
        ///
        ///     + jobStatus:
        ///
        ///         0: Fired (Bị đuổi việc)
        ///
        ///         1: Working (Đang làm việc)
        ///
        ///     + businessService:
        /// 
        ///         + serviceType:
        ///
        ///             1: Examination (Khám bệnh)
        ///
        ///             2: Treatment (Điều trị)
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("my-profile")]
        [Authorize]
        public async Task<ActionResult<BaseResponse<GetMyProfileResponse>>> GetMyProfile()
        {
            int userId = int.Parse(User.Claims.First(p => p.Type == "userId").Value);
            var response = await _userService.GetMyProfile(userId);
            return Ok(response);
        }


        [HttpPost]
        [Route("change-password/{userId}")]
        public async Task<ActionResult<BaseResponse<ChangePasswordResponse>>> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            return await _userService.ChangePassword(userId, oldPassword, newPassword);

        }
























        /*[HttpPost]
        public async Task<ActionResult<BaseResponse<CreateNewUserResponse>>> CreateUser([FromBody] CreateNewUserRequest request)
        {
            var user = await _userService.CreateUser(request);
            return user;
        }
        [HttpPost]
        [Route("base")]
        public async Task<ActionResult<BaseResponse<CreateNewUserResponse>>> CreateUserFromBase([FromBody] CreateNewUserRequest request)
        {
            var user = await _userService.CreateUserFromBase(request);
            return user;
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<GetUserByIdResponse>>> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }
        [HttpGet]
        [Route("base/{id}")]
        public async Task<ActionResult<BaseResponse<GetUserByIdResponse>>> GetUserByIdFromBase(int id)
        {
            return await _userService.GetUserByIdFromBase(id);
        }
        
        
        [HttpGet]
        [Route("base")]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllUserResponse>>>> GetAllUserFromBase()
        {
            var users = await _userService.GetAllUserFromBase();
            return Ok(users);
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<GetAllUserResponse>>>> GetAllUser()
        {
            var users = await _userService.GetAllUser();
            return users;
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<BaseResponse<DeleteUserResponse>>> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }
        [HttpDelete]
        [Route("base/{id}")]
        public async Task<ActionResult<BaseResponse<DeleteUserResponse>>> DeleteUserFromBase(int id)
        {
            return await _userService.DeleteUserFromBase(id);
        }
        
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<BaseResponse<UpdateUserResponse>>> UpdateUser(int id, [FromBody] UpdateUserRequest user)
        {
            return await _userService.UpdateUser(id, user);
        }
        [HttpPut]
        [Route("base/{id}")]
        public async Task<ActionResult<BaseResponse<UpdateUserResponse>>> UpdateUserFromBase(int id, [FromBody] UpdateUserRequest user)
        {
            return await _userService.UpdateUserFromBase(id, user);
        }*/
    }
}