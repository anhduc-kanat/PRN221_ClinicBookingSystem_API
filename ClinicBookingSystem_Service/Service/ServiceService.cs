﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Service;
using ClinicBookingSystem_Service.Models.Response.Service;
using ClinicBookingSystem_Service.Models.Response.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_BusinessObject.Enums;

namespace ClinicBookingSystem_Service.Service
{
    public class ServiceService:IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CreateServiceResponse>> CreateService(CreateServiceRequest request)
        {
            var service = _mapper.Map<BusinessService>(request);
            if (service.ServiceType == ServiceType.Examination)
            {
                service.IsPreBooking = true;
            }
            else
            {
                service.IsPreBooking = false;

            }
            /* service.IsPreBooking = service.IsPreBooking ?? false;
             if (service.IsPreBooking.Value)
             {
                 service.ServiceType = ServiceType.Examination;
             }*/
            await _unitOfWork.ServiceRepository.AddAsync(service);
            await _unitOfWork.SaveChangesAsync();
            var newServiceDto = _mapper.Map<CreateServiceResponse>(service);
            return new BaseResponse<CreateServiceResponse>("Add service successfully", StatusCodeEnum.OK_200, newServiceDto);
        }

        public async Task<BaseResponse<DeleteServiceResponse>> DeleteService(int id)
        {
            BusinessService businessService = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            await _unitOfWork.ServiceRepository.DeleteAsync(businessService);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<DeleteServiceResponse>(businessService);
            return new BaseResponse<DeleteServiceResponse>("Delete service successfully", StatusCodeEnum.OK_200, result);
        }

        public async Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllExamServices()
        {
            IEnumerable<BusinessService> services = await _unitOfWork.ServiceRepository.GetAllExamServices();
            var serviceResult = _mapper.Map<IEnumerable<GetServiceResponse>>(services);
            return new BaseResponse<IEnumerable<GetServiceResponse>>("Get All Exam Services successfully", StatusCodeEnum.OK_200, serviceResult);
        }

        public async Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllTreatmentServices()
        {
            IEnumerable<BusinessService> services = await _unitOfWork.ServiceRepository.GetAllTreatmentServices();
            var serviceResult = _mapper.Map<IEnumerable<GetServiceResponse>>(services);
            return new BaseResponse<IEnumerable<GetServiceResponse>>("Get All Exam Services successfully", StatusCodeEnum.OK_200, serviceResult);
        }
        
        public async Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllServices()
        {
            IEnumerable<BusinessService> services = await _unitOfWork.ServiceRepository.GetAllAsync();
            var servicesDto = _mapper.Map<IEnumerable<GetServiceResponse>>(services);
            return new BaseResponse<IEnumerable<GetServiceResponse>>("Get services successfully", StatusCodeEnum.OK_200,
                servicesDto);
        }

        public async Task<BaseResponse<GetServiceResponse>> GetServiceById(int id)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            var serviceDto = _mapper.Map<GetServiceResponse>(service);
            return new BaseResponse<GetServiceResponse>("Get service by id successfully", StatusCodeEnum.OK_200, serviceDto);
        }

        public async Task<BaseResponse<UpdateServiceResponse>> UpdateService(int id, UpdateServiceRequest request)
        {
            var existService = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            _mapper.Map(request, existService);
            if (existService.ServiceType == ServiceType.Examination)
            {
                existService.IsPreBooking = true;
            }
            else
            {
                existService.IsPreBooking = false;

            }
            await _unitOfWork.ServiceRepository.UpdateAsync(existService);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UpdateServiceResponse>(existService);
            return new BaseResponse<UpdateServiceResponse>("Update successfully", StatusCodeEnum.OK_200, result);
        }
    }
}