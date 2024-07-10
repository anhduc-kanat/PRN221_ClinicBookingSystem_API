﻿using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Response.Meeting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/meeting")]
public class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }
    
    /// <summary>
    /// Update meeting status bởi staff, login và truyền Bearer token vào header
    /// </summary>
    /// <remarks>
    /// 1: Done
    ///
    /// 2: CheckIn (Checkin để bắt đầu cuộc hẹn)
    ///
    /// 3: Waiting (Trong trạng thái chờ để được checkin)
    ///
    /// 4: Future (Cuộc hẹn sắp tới - không phải ngày hôm nay)
    /// </remarks>
    /// <param name="meetingId"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("update-meeting-status/{meetingId}")]
    public async Task<BaseResponse<UpdateMeetingResponse>> UpdateMeetingStatus(int meetingId, MeetingStatus status)
    {
        var result = await _meetingService.UpdateMeetingStatus(meetingId, status);
        return result;
    }
    
    /// <summary>
    /// Staff add dentist vào meeting, login và truyền Bearer token vào header
    /// </summary>
    /// <param name="meetingId"></param>
    /// <param name="dentistId"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("add-dentist-into-meeting/{meetingId}/{dentistId}")]
    [Authorize(Roles = "STAFF")]
    public async Task<BaseResponse<AddDentistIntoMeetingResponse>> AddDentistIntoMeeting(int meetingId, int dentistId)
    {
        var result = await _meetingService.AddDentistIntoMeeting(meetingId, dentistId);
        return result;
    }
    
    /// <summary>
    /// Staff và Customer đều có thể update dentist trong meeting
    /// </summary>
    /// <param name="meetingId"></param>
    /// <param name="dentistId"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("update-dentist-in-meeting/{meetingId}/{dentistId}")]
    [Authorize]
    public async Task<BaseResponse<UpdateDentistInMeeting>> UpdateDentistInMeeting(int meetingId, int dentistId)
    {
        var result = await _meetingService.UpdateDentistInMeeting(meetingId, dentistId);
        return result;
    }
}