﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject;
using ClinicBookingSystem_Repository.BaseRepositories;
using ClinicBookingSystem_Repository.IRepositories;
using System.Reflection.Metadata.Ecma335;

namespace ClinicBookingSystem_Repository.Repositories;

public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
{
    private readonly AppointmentDAO _appointmentDAO;
    public AppointmentRepository(AppointmentDAO appointmentDAO) : base(appointmentDAO)
    {
        _appointmentDAO = appointmentDAO;
    }
    public async Task<IEnumerable<Appointment>> GetAllAppointment()
    {
        return await _appointmentDAO.GetAllAppointment();
    }
    public async Task<Appointment> GetAppointmentById(int Id)
    {
        return await _appointmentDAO.GetAppointmentById(Id);
    }
    public async Task<IEnumerable<Appointment>> GetAllAppointmentPagination(int pageNumber, int pageSize)
    {
        return await _appointmentDAO.GetAllAppointmentPagination(pageNumber, pageSize);
    }
    public async Task<IEnumerable<Appointment>> GetAppointmentByUserId(int userId, int pageNumber, int pageSize)
    {
        return await _appointmentDAO.GetAppointmentByUserId(userId, pageNumber, pageSize);
    }

    public async Task<int> CountUserAppointment(int userId)
    {
        return await _appointmentDAO.CountUserAppointment(userId);
    }
    public async Task<IEnumerable<Appointment>> GetAppointmentByDatePagination(int pageNumber, int pageSize, DateOnly date)
    {
        return await _appointmentDAO.GetAppointmentByDayPagination(pageNumber, pageSize, date);
    }
    public async Task<IEnumerable<Appointment>> DentistGetTodayAppointment(int pageNumber, int pageSize, int dentistId, DateOnly date)
    {
        return await _appointmentDAO.DentistGetTodayAppointment(pageNumber, pageSize, dentistId, date);
    }
    public async Task<int> CountDentistAppointment(int dentistId, DateOnly date)
    {
        return await _appointmentDAO.CountDentistAppointment(dentistId, date);
    }

    public async Task<int> CountWhenStaffGetAppointmentByDate(DateOnly date)
    {
        return await _appointmentDAO.CountWhenStaffGetAppointmentByDate(date);
    }

    public async Task<IEnumerable<Appointment>> GetTodayMeetingTreatmentAppointment()
    {
        return await _appointmentDAO.GetTodayMeetingTreatmentAppointment();
    }

    public async Task<Appointment> GetAppointmentIfExistTreatmentMeeting(int appointmentId)
    {
        return await _appointmentDAO.GetAppointmentIfExistTreatmentMeeting(appointmentId);
    }

    public async Task<IEnumerable<Appointment>> CountAppointment(DateTime date, int type)
    {
        return await _appointmentDAO.CountAppointment(date, type);
    }
}