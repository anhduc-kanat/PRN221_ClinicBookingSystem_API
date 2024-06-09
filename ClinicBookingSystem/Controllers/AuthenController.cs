using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Response.Authen;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenController: ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAuthenService _authenService;

        public AuthenController(ICustomerService customerService, IAuthenService authenService)
        {
            _customerService = customerService;
            _authenService = authenService;
        }
        [HttpPost]
        [Route ("register")]
        public async Task<  ActionResult<BaseResponse<RegisterResponse>>> register([FromBody] RegisterRequest request)
        {
            var user = await _customerService.AddCustomer(request);
            return user;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {

            var token = await _authenService.LoginWithJwtTokenAsync(request);
            return token;

        }

        [HttpPost]
        [Route("renew")]
        public async Task<ActionResult<BaseResponse<LoginResponse>>> RenewAccessToken([FromBody] RenewTokenRequest request)
        {

            var token = await _authenService.RenewJwtTokenAsync(request);
            return token;

        }

    }
}
