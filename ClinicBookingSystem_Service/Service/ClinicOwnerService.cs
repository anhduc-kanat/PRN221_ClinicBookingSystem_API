﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Clinic_Owner;
using ClinicBookingSystem_Service.Models.Response.Clinic_Owner;
namespace ClinicBookingSystem_Service.Service;

public class ClinicOwnerService : IClinicOwnerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public ClinicOwnerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseResponse<GetClinicOwnerResponse>> GetClinicOwner(int id)
    {
        try
        {
            User clinicOwner = await _unitOfWork.ClinicOwnerRepository.GetByIdAsync(id);
            GetClinicOwnerResponse response = _mapper.Map<GetClinicOwnerResponse>(clinicOwner);
            return new BaseResponse<GetClinicOwnerResponse>("Get Dentist By ID successfully!", StatusCodeEnum.OK_200, response);
        }
        catch (Exception ex)
        {
            return new BaseResponse<GetClinicOwnerResponse>("Error at GetDentistById Service: " + ex.Message , StatusCodeEnum.InternalServerError_500);
        }
    }

    public async Task<BaseResponse<GetClinicOwnerResponse>> CreateClinicOwner(CreateClinicOwnerRequest request)
    {
        
        try
        {
            HashPassword hash = new HashPassword();
            request.Password = hash.EncodePassword(request.Password);
            User user = _mapper.Map<User>(request);
            Role role = await _unitOfWork.RoleRepository.GetRoleByName("CLINIC_OWNER");
            user.Role = role;
            var createdUser = await _unitOfWork.ClinicOwnerRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<GetClinicOwnerResponse
            >("Create Clinic Owner Successfully!", StatusCodeEnum.Created_201);
        }
        catch (Exception ex)
        {
            return new BaseResponse<GetClinicOwnerResponse>("Error at CreateClinicOwner Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
        }
    }

    public async Task<BaseResponse<GetClinicOwnerResponse>> UpdateClinicOwner(int id, UpdateClinicOwnerRequest request)
    {
    try
    {
        User clinicOwner = await _unitOfWork.ClinicOwnerRepository.GetByIdAsync(id);
        _mapper.Map(request, clinicOwner);
        User updatedDentist = await _unitOfWork.ClinicOwnerRepository.UpdateAsync(clinicOwner);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<GetClinicOwnerResponse>("Update Dentist Successfully!", StatusCodeEnum.OK_200);
    }
    catch (Exception ex)
    {
        return new BaseResponse<GetClinicOwnerResponse>("Error at UpdateDentist Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
    }
    }

    public async Task<BaseResponse<GetClinicOwnerResponse>> DeleteClinicOwner(int id)
    {
        try
        {
            User clinicOwner = await _unitOfWork.DentistRepository.GetByIdAsync(id);
            await _unitOfWork.ClinicOwnerRepository.DeleteAsync(clinicOwner);
            var response = _mapper.Map<GetClinicOwnerResponse>(clinicOwner);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse<GetClinicOwnerResponse>("Delete Dentist Successfully!", StatusCodeEnum.OK_200, response);
        }
        catch (Exception ex)
        {
            return new BaseResponse<GetClinicOwnerResponse>("Error at DeleteDentist Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
        }
    }
}