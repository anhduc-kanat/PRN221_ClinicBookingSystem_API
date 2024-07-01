using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.DTOs.Appointment;
using ClinicBookingSystem_Service.Models.DTOs.AppointmentBusinessService;
using ClinicBookingSystem_Service.Models.Request.Appointment;
using ClinicBookingSystem_Service.Models.Response.Appointment;

namespace ClinicBookingSystem_Service.Mapping;

public class MappingAppointment : Profile
{
    public MappingAppointment()
    {
        //AppointmentDto
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        
        //CRUD Appointment
        CreateMap<Appointment, CreateAppointmentRequest>();
        CreateMap<Appointment, UpdateAppointmentRequest>();
        CreateMap<Appointment, CreateAppointmentResponse>().ReverseMap();
        CreateMap<Appointment, UpdateAppointmentResponse>().ReverseMap();
        CreateMap<Appointment, DeleteAppointmentResponse>().ReverseMap();
        CreateMap<Appointment, GetAppointmentResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot.Name))
            .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.Slot.StartAt))
            .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.Slot.EndAt))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.UserTreatmentName))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserTreatmentId))
            .ForMember(dest => dest.PatientAddress, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Address))
            .ForMember(dest => dest.PatientGender, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Gender))
            .ForMember(dest => dest.PatientType, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Type))
            .ForMember(dest => dest.PatientPhoneNumber, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).PhoneNumber))
            .ForMember(dest => dest.PatientDateOfBirth, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).DateOfBirth))
            .ForMember(dest => dest.PatientCCCD, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).CCCD));
        CreateMap<StaffGetAppointmentByDayResponse, Appointment>().ReverseMap()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot.Name))
            .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.Slot.StartAt))
            .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.Slot.EndAt))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.UserTreatmentName))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserTreatmentId))
            .ForMember(dest => dest.PatientAddress, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Address))
            .ForMember(dest => dest.PatientGender, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Gender))
            .ForMember(dest => dest.PatientType, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Type))
            .ForMember(dest => dest.PatientPhoneNumber, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).PhoneNumber))
            .ForMember(dest => dest.PatientDateOfBirth, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).DateOfBirth))
            .ForMember(dest => dest.PatientCCCD, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).CCCD)); 

        //Customer booking appointment
        CreateMap<CustomerBookingAppointmentResponse, Appointment>().ReverseMap()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<StaffBookingAppointmentResponse, Appointment>().ReverseMap()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id));
        
        CreateMap<Appointment, CustomerBookingAppointmentRequest>();
        //Get appointment by user id
        CreateMap<Appointment, UserGetAppointmentResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot.Name))
            .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.Slot.StartAt))
            .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.Slot.EndAt))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.UserTreatmentName))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserTreatmentId))
            .ForMember(dest => dest.PatientAddress, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Address))
            .ForMember(dest => dest.PatientGender, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Gender))
            .ForMember(dest => dest.PatientType, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Type))
            .ForMember(dest => dest.PatientPhoneNumber, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).PhoneNumber))
            .ForMember(dest => dest.PatientDateOfBirth, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).DateOfBirth))
            .ForMember(dest => dest.PatientCCCD, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).CCCD));
        
        CreateMap<DentistGetTodayAppointmentsResponse, Appointment>().ReverseMap()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)))
            .ForMember(dest => dest.SlotName, opt => opt.MapFrom(src => src.Slot.Name))
            .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.Slot.StartAt))
            .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.Slot.EndAt))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.UserTreatmentName))
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.UserTreatmentId))
            .ForMember(dest => dest.PatientAddress, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Address))
            .ForMember(dest => dest.PatientGender, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Gender))
            .ForMember(dest => dest.PatientType, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).Type))
            .ForMember(dest => dest.PatientPhoneNumber, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).PhoneNumber))
            .ForMember(dest => dest.PatientDateOfBirth, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).DateOfBirth))
            .ForMember(dest => dest.PatientCCCD, opt =>
                opt.MapFrom(src =>
                    src.Users.FirstOrDefault(p =>
                        p.Id == src.UserAccountId).UserProfiles.FirstOrDefault(p =>
                        p.Id == src.UserTreatmentId).CCCD)); 

        //Check-in
        CreateMap<StaffUpdateAppointmentStatusResponse, Appointment>().ReverseMap();
        CreateMap<AppointmentBusinessService, Appointment>().ReverseMap();
        CreateMap<AppointmentBusinessServiceDto, Appointment>().ReverseMap();
        CreateMap<AppointmentBusinessService, AppointmentBusinessServiceDto>().ReverseMap();
        //
        CreateMap<AppointmentBusinessServiceDto, Appointment>().ReverseMap();
    }
}