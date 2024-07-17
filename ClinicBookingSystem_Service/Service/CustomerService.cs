using AutoMapper;
using Azure;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Authen;
using ClinicBookingSystem_Service.Models.Request.Customer;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Customer;
using ClinicBookingSystem_Service.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.Models.Request.Relative;
using MassTransit;
using ClinicBookingSystem_Service.Common.Utils;

namespace ClinicBookingSystem_Service.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HashPassword _hash;
        public CustomerService(IUnitOfWork unitOfWork,IMapper iMapper, HashPassword hash)
        {
            _unitOfWork = unitOfWork;
            _mapper = iMapper;
            _hash = hash;

        }

        public async Task<BaseResponse<RegisterResponse>> AddCustomer(RegisterRequest request)
        {
            try
            {
                bool exist =await _unitOfWork.CustomerRepository.GetCustomerByPhone(request.PhoneNumber);
                if (exist)
                {
                    return new BaseResponse<RegisterResponse>("Phone was existed", StatusCodeEnum.BadRequest_400);
                }
                request.Password = _hash.EncodePassword(request.Password);
                Role role = await _unitOfWork.RoleRepository.GetRoleByName("CUSTOMER");
                User customerAddData = _mapper.Map<User>(request);
                customerAddData.Role = role;
                var userProfileRequest = _mapper.Map<CreateUserProfileRequest>(customerAddData);
                UserProfile userProfile = _mapper.Map<UserProfile>(userProfileRequest);
                userProfile.User = customerAddData;
                var user = await _unitOfWork.CustomerRepository.AddAsync(customerAddData);
                await _unitOfWork.UserProfileRepository.AddAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<RegisterResponse>(user);
                return new BaseResponse<RegisterResponse>("Add Succesfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<RegisterResponse>("Customer Service " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
            
        }


        public async Task<BaseResponse<DeleteCustomerResponse>> DeleteCustomer(int id)
        {
            try
            {
                User customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if(customer == null)
                {
                     return new BaseResponse<DeleteCustomerResponse>("Customer not found", StatusCodeEnum.BadRequest_400);
                }
                await _unitOfWork.CustomerRepository.DeleteAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<DeleteCustomerResponse>(customer);
                return new BaseResponse<DeleteCustomerResponse>("Delete Succesfully", StatusCodeEnum.OK_200,response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<DeleteCustomerResponse>("Customer Service " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }

        }

        public async Task<BaseResponse<IEnumerable<GetCustomerResponse>>> GetAllCustomer()
        {
            try
            {
                Role role = await _unitOfWork.RoleRepository.GetRoleByName("CUSTOMER");
                IEnumerable<User> customers = await _unitOfWork.CustomerRepository.GetAllCustomer(role.Id);
                var response = _mapper.Map<IEnumerable<GetCustomerResponse>>(customers);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<IEnumerable<GetCustomerResponse>>("Get all customer successfully", StatusCodeEnum.OK_200,response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetCustomerResponse>>("Customer Service " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<GetCustomerResponse>> GetCustomerById(int id)
        {
            try
            {
                Role role = await _unitOfWork.RoleRepository.GetRoleByName("CUSTOMER");
                User customer = await _unitOfWork.CustomerRepository.GetCustomerById(role.Id,id);
                var response = _mapper.Map<GetCustomerResponse>(customer);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<GetCustomerResponse>("Get all customer successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetCustomerResponse>("Customer Service " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<UpdateCustomerResponse>> UpdateCustomer(int id, UpdateCustomerRequest request)
        {
            try
            {
                User customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                _mapper.Map(request,customer);
                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                var response = _mapper.Map<UpdateCustomerResponse>(customer);
                return new BaseResponse<UpdateCustomerResponse>("Update Succesfully", StatusCodeEnum.Created_201, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UpdateCustomerResponse>("Customer Service " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }


     

    }
}
