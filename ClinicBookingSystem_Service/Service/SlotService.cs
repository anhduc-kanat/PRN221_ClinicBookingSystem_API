﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Repository.Repositories;
using ClinicBookingSystem_Service.IService;
using AutoMapper;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.DTOs.Slot;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Request.Slot;
using ClinicBookingSystem_Service.Models.Response.Slot;
using ClinicBookingSystem_Service.Models.Response.User;
using ClinicBookingSystem_Service.Models.Request.User;
using ClinicBookingSystem_Service.Models.Response.Appointment;

namespace ClinicBookingSystem_Service.Service
{
    public class SlotService : ISlotService
    {
        //private readonly ISlotRepository _slotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BaseResponse<IEnumerable<SlotResponse>>> GetAllSlots()
        {
            IEnumerable<Slot> slots = await _unitOfWork.SlotRepository.GetAllAsync();
            return new BaseResponse<IEnumerable<SlotResponse>>("Get slots successfully", StatusCodeEnum.OK_200,
                _mapper.Map<IEnumerable<SlotResponse>>(slots));
        }

        public async Task<BaseResponse<SlotResponse>> GetSlotById(int id)
        {
            var slot = await _unitOfWork.SlotRepository.GetByIdAsync(id);
            var slotDto = _mapper.Map<SlotResponse>(slot);
            return new BaseResponse<SlotResponse>("Get slot by id successfully", StatusCodeEnum.OK_200, slotDto);
        }

        public async Task<BaseResponse<SlotResponse>> CreateSlot(CreateNewSlotRequest request)
        {
            TimeSpan StartTime = new TimeSpan(request.StartAtHour, request.StartAtMinute, 0);
            TimeSpan EndTime = new TimeSpan(request.EndAtHour, request.EndAtMinute, 0);
            
            if (EndTime > new TimeSpan(19, 0, 0) || StartTime > new TimeSpan(19, 0, 0))
            {
                throw new CoreException("Slot cannot create or end after 19:00", StatusCodeEnum.BadRequest_400);
            }
            
            var existSlot = await _unitOfWork.SlotRepository.GetSlotByTime(StartTime, EndTime);
            if (existSlot != null) throw new CoreException("Slot is already exist", StatusCodeEnum.Conflict_409);
            
            bool isMorningShift = request.StartAtHour < 12;
            SlotDto slotDto = new SlotDto
            {
                Name = request.Name,
                Description = request.Description,
                StartAt = StartTime,
                EndAt = EndTime,
                IsMorningShift = isMorningShift,
            };
            var slot = _mapper.Map<Slot>(slotDto);
            await _unitOfWork.SlotRepository.AddAsync(slot);
            await _unitOfWork.SaveChangesAsync();
            var newSlotDto = _mapper.Map<SlotResponse>(slot);
            return new BaseResponse<SlotResponse>("Add slot successfully", StatusCodeEnum.OK_200, newSlotDto);
        }
        

        public async Task<BaseResponse<SlotResponse>> DeleteSlot(int id)
        {
            var slot = await _unitOfWork.SlotRepository.DeleteSlot(id);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<SlotResponse>(slot);
            return new BaseResponse<SlotResponse>("Delete slot successfully", StatusCodeEnum.OK_200, result);
        }

        public async Task<BaseResponse<SlotResponse>> UpdateSlot(int id, UpdateSlotRequest request)
        {
              TimeSpan StartTime = new TimeSpan((int)request.StartAtHour, (int)request.StartAtMinute, 0);
            TimeSpan EndTime = new TimeSpan((int)request.EndAtHour, (int)request.EndAtMinute, 0);
            if (EndTime > new TimeSpan(19, 0, 0) || StartTime > new TimeSpan(19, 0, 0))
            {
                throw new CoreException("Slot cannot update starttime or endtime after 19:00", StatusCodeEnum.BadRequest_400);
            }
            var availableSlot = await _unitOfWork.SlotRepository.GetSlotByTimeExceptCurrentSlot(id, StartTime, EndTime);
            if (availableSlot != null) throw new CoreException("Slot is already exist", StatusCodeEnum.Conflict_409);

            var existSlot = await _unitOfWork.SlotRepository.GetByIdAsync(id);
            existSlot.IsMorningShift = request.StartAtHour < 12;
            _mapper.Map(request, existSlot);
            await _unitOfWork.SlotRepository.UpdateAsync(existSlot);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<SlotResponse>(existSlot);
            return new BaseResponse<SlotResponse>("Update successfully", StatusCodeEnum.OK_200, result);
        }

        public async Task<BaseResponse<IEnumerable<SlotResponse>>> CheckSlotAvailable(int dentistId, DateTime dateTime)
        {
            var slots = await _unitOfWork.SlotRepository.CheckAvailableSlot(dentistId, dateTime);
            var response = _mapper.Map<IEnumerable<SlotResponse>>(slots);
            return new BaseResponse<IEnumerable<SlotResponse>> ("Update successfully", StatusCodeEnum.OK_200, response);

        }
    }
}