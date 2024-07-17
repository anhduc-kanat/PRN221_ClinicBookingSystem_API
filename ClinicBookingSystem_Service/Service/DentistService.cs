﻿using AutoMapper;
using Azure.Core;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IServices;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Dentist;
using ClinicBookingSystem_Service.Models.Response.Authen;
using ClinicBookingSystem_Service.Models.Response.Dentist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.Notification.EmailNotification.Service;
using ClinicBookingSystem_Service.RabbitMQ.Events.EmailNotification;
using ClinicBookingSystem_Service.RabbitMQ.IService;

namespace ClinicBookingSystem_Service.Services
{
    public class DentistService : IDentistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRabbitMQBus _rabbitMQBus;
        private readonly HashPassword _hash;
        private readonly GeneratePassword _generate;
        public DentistService(IUnitOfWork unitOfWork, IMapper mapper, HashPassword hash, GeneratePassword generate, IRabbitMQBus rabbitMqBus)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rabbitMQBus = rabbitMqBus;
            _hash = hash;
            _generate = generate;
        }

        public async Task<BaseResponse<CreateDentistResponse>> CreateDentist(CreateDentistRequest request)
        {
            try
            {
                string unhashedPassword = _generate.generatePassword();
                bool exist = await _unitOfWork.CustomerRepository.GetCustomerByPhone(request.PhoneNumber);
                var unhashedPassword = request.Password;
                if (exist)
                {
                    return new BaseResponse<CreateDentistResponse>("Phone was existed", StatusCodeEnum.BadRequest_400);

                }
                User user = _mapper.Map<User>(request);
                user.Password = _hash.EncodePassword(unhashedPassword);
                Role role = await _unitOfWork.RoleRepository.GetRoleByName("DENTIST");
                user.Role = role;
                user.IsBusy = false;
                List<BusinessService> services = new List<BusinessService>();
                foreach (int serviceId in request.ServicesId)
                {
                    var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);
                    if (service != null)
                    {
                        services.Add(service);
                    }
                }
                var createdUser = await _unitOfWork.DentistRepository.CreateNewDentist(user, services);
                await _unitOfWork.SaveChangesAsync();

                await _rabbitMQBus.PublishAsync(new EmailNotificationEvent()
                {
                    Title = "Welcome to DuckClinic",
                    Subject = "Create account successfully at DuckClinic",
                    PhoneNumber = $"{user.PhoneNumber}",
                    Password = unhashedPassword,
                    To = $"{user.Email}",
                    ViewUrl = "./View/NotificationEmailTemplate.cshtml"
                });
                return new BaseResponse<CreateDentistResponse>("Create Dentist Successfully!",
                    StatusCodeEnum.Created_201);
            }
            catch (Exception ex)
            {
                return new BaseResponse<CreateDentistResponse>("Error at CreateDentist Service: " + ex.Message,
                    StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<DeleteDentistResponse>> DeleteDentist(int id)
        {
            try
            {
                User dentist = await _unitOfWork.DentistRepository.GetByIdAsync(id);
                await _unitOfWork.DentistRepository.DeleteAsync(dentist);
                var response = _mapper.Map<DeleteDentistResponse>(dentist);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<DeleteDentistResponse>("Delete Dentist Successfully!", StatusCodeEnum.OK_200,
                    response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<DeleteDentistResponse>("Error at DeleteDentist Service: " + ex.Message,
                    StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentists()
        {
            try
            {
                IEnumerable<User> dentists = await _unitOfWork.DentistRepository.GetDentistsByRole();
                IEnumerable<GetAllDentistsResponse> response =
                    _mapper.Map<IEnumerable<GetAllDentistsResponse>>(dentists);
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>("Get All Dentists successfully",
                    StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>(
                    "Error at GetAllDentists Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentistsByService(string serviceName)
        {
            try
            {
                IEnumerable<User> dentists = await _unitOfWork.DentistRepository.GetDentistsByService(serviceName);
                IEnumerable<GetAllDentistsResponse> response =
                    _mapper.Map<IEnumerable<GetAllDentistsResponse>>(dentists);
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>("Get All Dentists successfully",
                    StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>(
                    "Error at GetAllDentists Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentistsByServiceId(int serviceId)
        {
            try
            {
                IEnumerable<User> dentists = await _unitOfWork.DentistRepository.GetDentistsByServiceId(serviceId);
                IEnumerable<GetAllDentistsResponse> response =
                    _mapper.Map<IEnumerable<GetAllDentistsResponse>>(dentists);
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>("Get All Dentists successfully",
                    StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<GetAllDentistsResponse>>(
                    "Error at GetAllDentists Service: " + ex.Message, StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<IEnumerable<DateTime>>> GetAvailableDate(int id)
        {
            try
            {
                IEnumerable<DateTime> response = await _unitOfWork.DentistRepository.GetAvailableDate(id);
                return new BaseResponse<IEnumerable<DateTime>>("Get Dentist By ID successfully!", StatusCodeEnum.OK_200,
                    response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<DateTime>>("Error at GetDentistById Service: " + ex.Message,
                    StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<GetDentistByIdResponse>> GetDentistById(int id)
        {
            try
            {
                User dentist = await _unitOfWork.DentistRepository.GetDentistById(id);
                GetDentistByIdResponse response = _mapper.Map<GetDentistByIdResponse>(dentist);
                return new BaseResponse<GetDentistByIdResponse>("Get Dentist By ID successfully!",
                    StatusCodeEnum.OK_200, response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<GetDentistByIdResponse>("Error at GetDentistById Service: " + ex.Message,
                    StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<UpdateDentistResponse>> UpdateDentist(int id, UpdateDentistRequest request)
        {
            try
            {
                /*HashPassword hash = new HashPassword();
                request.Password = hash.EncodePassword(request.Password);*/
                User dentist = await _unitOfWork.DentistRepository.GetDentistById(id);
                if (request.ServicesId.Count != 0)
                {
                    List<int> servicesId = request.ServicesId;
                    _mapper.Map(request, dentist);
                    var serviceRemove = dentist.BusinessServices.Where(s => !servicesId.Contains(s.Id)).ToList();
                    foreach (var service in serviceRemove)
                    {
                        dentist.BusinessServices.Remove(service);
                    }
                    foreach (var serviceId in servicesId)
                    {
                        if (!dentist.BusinessServices.Any(ds => ds.Id == serviceId))
                        {
                            var newBusinessService = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);
                            dentist.BusinessServices.Add(newBusinessService);
                        }
                    }
                }
                User updatedDentist = await _unitOfWork.DentistRepository.UpdateAsync(dentist);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<UpdateDentistResponse>("Update Dentist Successfully!", StatusCodeEnum.OK_200);
            }
            catch (Exception ex)
            {
                return new BaseResponse<UpdateDentistResponse>("Error at UpdateDentist Service: " + ex.Message,
                    StatusCodeEnum.InternalServerError_500);
            }
        }

        public async Task<BaseResponse<AddDentistToBusinessServiceResponse>> AddDentistToService(int dentistId,
            int businessServiceId)
        {
                User dentist = await _unitOfWork.DentistRepository.GetDentistById(dentistId);
                if (dentist == null) throw new CoreException("Dentist not found!", StatusCodeEnum.BadRequest_400);
                BusinessService service = await _unitOfWork.ServiceRepository.GetServiceById(businessServiceId);
                if (service == null) throw new CoreException("Service not found!", StatusCodeEnum.BadRequest_400);

                dentist.BusinessServices.Add(service);
                service.Users.Add(dentist);
                
                await _unitOfWork.DentistRepository.UpdateAsync(dentist);
                await _unitOfWork.ServiceRepository.UpdateAsync(service);
                
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<AddDentistToBusinessServiceResponse>("Add Dentist to Service Successfully!",
                    StatusCodeEnum.OK_200);
        }
    }
}