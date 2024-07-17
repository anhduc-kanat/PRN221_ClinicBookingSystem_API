using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Request.Customer;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    [Authorize(Roles ="STAFF,CUSTOMER")]
    public class CustomerController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPut]
        [Route("update-customer/{id}")]
        public async Task<ActionResult<BaseResponse<UpdateCustomerResponse>>> Update(int id, [FromBody] UpdateCustomerRequest request)
        {
            return await _customerService.UpdateCustomer(id,request);
        }

        [HttpDelete]
        [Route("delete-customer/{id}")]
        public async Task<ActionResult<BaseResponse<DeleteCustomerResponse>>> Delete(int id)
        {
            return await _customerService.DeleteCustomer(id);
        }

        [HttpGet]
        [Route("get-customer-by-id/{id}")]
        public async Task<ActionResult<BaseResponse<GetCustomerResponse>>> GetById(int id)
        {
            var user = await _customerService.GetCustomerById(id);
            return user;
        }

        [HttpGet]
        [Route("get-all-customers")]
        public async Task<ActionResult<BaseResponse<IEnumerable< GetCustomerResponse>>>> GetAll()
        {
            return await _customerService.GetAllCustomer();
            
        }

     

    }
}
