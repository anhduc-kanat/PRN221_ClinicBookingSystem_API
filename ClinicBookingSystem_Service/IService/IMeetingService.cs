﻿using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Response.Meeting;

namespace ClinicBookingSystem_Service.IService;

public interface IMeetingService
{
    Task<BaseResponse<UpdateMeetingResponse>> UpdateMeetingStatus(int meetingId, MeetingStatus status);
    Task<BaseResponse<AddDentistIntoMeetingResponse>> AddDentistIntoMeeting(int meetingId, int dentistId);
    Task<BaseResponse<UpdateDentistInMeeting>> UpdateDentistInMeeting(int meetingId, int dentistId);
}