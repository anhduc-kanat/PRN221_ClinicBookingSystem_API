using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Request.Customer;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.IService
{
    public interface ICustomerService
    {
        Task<BaseResponse<RegisterResponse>> AddCustomer(RegisterRequest request);
        Task<BaseResponse<UpdateCustomerResponse>> UpdateCustomer(int id, UpdateCustomerRequest request);
        Task<BaseResponse<DeleteCustomerResponse>> DeleteCustomer(int id);
        Task<BaseResponse<IEnumerable< GetCustomerResponse>>> GetAllCustomer();
        Task<BaseResponse<GetCustomerResponse>> GetCustomerById(int id);

    }
}
