﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Repository.Repositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Relative;
using ClinicBookingSystem_Service.Models.Request.UserProfile;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.CustomException;
using Microsoft.IdentityModel.Tokens;

namespace ClinicBookingSystem_Service.Service
{
    internal class UserProfileService : IUserProfileService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<CreateUserProfileResponse>> AddUserProfile(int userId, CreateUserProfileRequest request)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                UserProfile data = _mapper.Map<UserProfile>(request);
                data.User = user;
                await _unitOfWork.UserProfileRepository.AddAsync(data);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CreateUserProfileResponse>("Successfully", StatusCodeEnum.Created_201);
            }
            catch (Exception ex)
            {

                return new BaseResponse<CreateUserProfileResponse>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }

        public async Task<BaseResponse<DeleteUserProfileResponse>> DeleteUserProfile(int id)
        {
            try
            {
                UserProfile userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(id);
                if(userProfile == null)
                {
                    return new BaseResponse<DeleteUserProfileResponse>("User Profile not found", StatusCodeEnum.BadRequest_400);
                }
                await _unitOfWork.UserProfileRepository.DeleteAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<DeleteUserProfileResponse>("Successfully", StatusCodeEnum.Created_201);

            }
            catch (Exception ex)
            {
                return new BaseResponse<DeleteUserProfileResponse>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }

        public async Task<BaseResponse<GetUserProfileResponse>> GetUserProfile(int id)
        {
            try
            {
                UserProfile userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(id);
                if(userProfile == null)
                {
                    return new BaseResponse<GetUserProfileResponse>("User Profile not found", StatusCodeEnum.BadRequest_400);
                }
                var response = _mapper.Map<GetUserProfileResponse>(userProfile);
                return new BaseResponse<GetUserProfileResponse>("Successfully", StatusCodeEnum.OK_200, response);

            }
            catch (Exception ex)
            {

                return new BaseResponse<GetUserProfileResponse>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }

        public async Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfiles()
        {
            try
            {
                IEnumerable<UserProfile> userProfile = await _unitOfWork.UserProfileRepository.GetAllAsync();
                var response = _mapper.Map<IEnumerable<GetUserProfileResponse>>(userProfile);
                return new BaseResponse<IEnumerable< GetUserProfileResponse>>("Successfully", StatusCodeEnum.Created_201,response);

            }
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<GetUserProfileResponse>>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }

        public async Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfilesByUser(string phone)
        {
            try
            {
                IEnumerable<UserProfile> userProfile = await _unitOfWork.UserProfileRepository.GetUserProfilesByUser(phone);
                var response = _mapper.Map<IEnumerable<GetUserProfileResponse>>(userProfile);
                return new BaseResponse<IEnumerable<GetUserProfileResponse>>("Successfully", StatusCodeEnum.OK_200, response);

            }
            catch (Exception ex)
            {

                return new BaseResponse<IEnumerable<GetUserProfileResponse>>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }

        public async Task<BaseResponse<UpdateUserProfileResponse>> UpdateUserProfile(int id, UpdateUserProfileRequest request)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
                UserProfile userProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(id);
                UserProfile data = _mapper.Map(request, userProfile);
                data.User = user;
                await _unitOfWork.UserProfileRepository.UpdateAsync(data);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<UpdateUserProfileResponse>("Successfully", StatusCodeEnum.Created_201);
            }
            catch (Exception ex)
            {

                return new BaseResponse<UpdateUserProfileResponse>("User Profile Service " + ex.Message, StatusCodeEnum.InternalServerError_500);

            }
        }
        
        public async Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfileByUserId(int userId)
        {
                IEnumerable<UserProfile> userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileById(userId);
                var response = _mapper.Map<IEnumerable<GetUserProfileResponse>>(userProfile);
                return new BaseResponse<IEnumerable<GetUserProfileResponse>>("Successfully", StatusCodeEnum.OK_200,
                    response);
        }
        public async Task<BaseResponse<IEnumerable<GetUserProfileResponse>>> GetUserProfileByUserAccountId(int userId)
        {
            IEnumerable<UserProfile> userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileById(userId);
            if(userProfile.IsNullOrEmpty()) throw new CoreException("This user has no profiles", StatusCodeEnum.BadRequest_400);
            var response = _mapper.Map<IEnumerable<GetUserProfileResponse>>(userProfile);
            return new BaseResponse<IEnumerable<GetUserProfileResponse>>("Successfully", StatusCodeEnum.OK_200,
                response);
        }
    }
}