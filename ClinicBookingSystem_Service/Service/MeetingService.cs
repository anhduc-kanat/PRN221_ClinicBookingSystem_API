﻿using AutoMapper;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Response.Meeting;

namespace ClinicBookingSystem_Service.Service;

public class MeetingService : IMeetingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public MeetingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BaseResponse<UpdateMeetingResponse>> UpdateMeetingStatus(int meetingId, MeetingStatus status)
    {
        var meeting = await _unitOfWork.MeetingRepository.GetByIdAsync(meetingId);
        if(meeting == null) throw new CoreException("Meeting not found", StatusCodeEnum.BadRequest_400);
        if (meeting.Date.Value.Year > DateTime.Now.Year &&
            meeting.Date.Value.Month > DateTime.Now.Month &&
            meeting.Date.Value.Day > DateTime.Now.Day) throw new CoreException("Meeting is in the future", StatusCodeEnum.BadRequest_400);
        if(status == MeetingStatus.Done) throw new CoreException("Dont have permission to do this", StatusCodeEnum.BadRequest_400);
        meeting.Status = status;
        await _unitOfWork.MeetingRepository.UpdateAsync(meeting);
        await _unitOfWork.SaveChangesAsync();
        var result = _mapper.Map<UpdateMeetingResponse>(meeting);
        return new BaseResponse<UpdateMeetingResponse>("Update meeting status successfully", StatusCodeEnum.OK_200, result);
    }
}