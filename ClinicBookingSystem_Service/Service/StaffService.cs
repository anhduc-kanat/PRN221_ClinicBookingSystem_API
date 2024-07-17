using AutoMapper;
using Azure.Core;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Staff;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.Service
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HashPassword _hash;
        private readonly GeneratePassword _generate;
        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, HashPassword hash, GeneratePassword generate)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hash = hash;
            _generate = generate;
        }
        public async Task<BaseResponse<CreateStaffResponse>> CreateStaff(CreateStaffRequest request)
        {
            try
            {
                string unhashedPassword = _generate.generatePassword();
                bool exist = await _unitOfWork.CustomerRepository.GetCustomerByPhone(request.PhoneNumber);
                if (exist)
                {
                    return new BaseResponse<CreateStaffResponse>("Phone was existed", StatusCodeEnum.BadRequest_400);
                }
                User user = _mapper.Map<User>(request);
                user.Password = _hash.EncodePassword(unhashedPassword);
                Role role = await _unitOfWork.RoleRepository.GetRoleByName("STAFF");
                user.Role = role;
                var createdUser = await _unitOfWork.StaffRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CreateStaffResponse>("Create Staff Successfully!", StatusCodeEnum.Created_201);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CreateStaffResponse>("Error at CreateStaff Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<DeleteStaffResponse>> DeleteStaff(int id)
        {
            try
            {
                User staff = await _unitOfWork.StaffRepository.GetByIdAsync(id);
                await _unitOfWork.StaffRepository.DeleteAsync(staff);
                var response = _mapper.Map<DeleteStaffResponse>(staff);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<DeleteStaffResponse>("Delete Staff Successfully!", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<DeleteStaffResponse>("Error at DeleteStaff Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<IEnumerable<GetAllStaffsResponse>>> GetAllStaffs()
        {
            try
            {
                IEnumerable<User> staffs = await _unitOfWork.StaffRepository.GetStaffsByRole();
                IEnumerable<GetAllStaffsResponse> response = _mapper.Map<IEnumerable<GetAllStaffsResponse>>(staffs);
                return new BaseResponse<IEnumerable<GetAllStaffsResponse>>("Get All Staffs successfully", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetAllStaffsResponse>>("Error at GetAllStaffs Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<GetStaffByIdResponse>> GetStaffById(int id)
        {
            try
            {
                User staff = await _unitOfWork.StaffRepository.GetByIdAsync(id);
                GetStaffByIdResponse response = _mapper.Map<GetStaffByIdResponse>(staff);
                return new BaseResponse<GetStaffByIdResponse>("Get Staff By ID successfully!", StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetStaffByIdResponse>("Error at GetStaffById Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<UpdateStaffResponse>> UpdateStaff(int id, UpdateStaffRequest request)
        {
            try
            {
                /*HashPassword hash = new HashPassword();
                request.Password = hash.EncodePassword(request.Password);*/
                User staff = await _unitOfWork.StaffRepository.GetByIdAsync(id);
                _mapper.Map(request, staff);
                User updatedStaff = await _unitOfWork.StaffRepository.UpdateAsync(staff);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<UpdateStaffResponse>("Update Staff Successfully!", StatusCodeEnum.OK_200);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UpdateStaffResponse>("Error at UpdateStaff Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }
    }
}
