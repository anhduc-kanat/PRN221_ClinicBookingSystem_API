﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_BusinessObject.Enums;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class ServiceDAO : BaseDAO<BusinessService>
{
    private readonly ClinicBookingSystemContext _context;
    public ServiceDAO(ClinicBookingSystemContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BusinessService>> GetAllExamServices()
    {
        return await _context.BusinessServices.Where(s => s.ServiceType == ServiceType.Examination && s.IsPreBooking == true).ToListAsync();
    }

    public async Task<IEnumerable<BusinessService>> GetAllTreatmentServices()
    {
        return await GetQueryableAsync()
            .Where(p => p.ServiceType == ServiceType.Treatment)
            .ToListAsync();
    }
    public async Task<IEnumerable<BusinessService>> GetAllServices()
    {
        return await _context.BusinessServices.ToListAsync();
    }
    //
    public async Task<BusinessService> GetServiceById(int id)
    {
        return await GetQueryableAsync()
            .Include(p => p.Users)
            .ThenInclude(p => p.Role)
            .Include(p => p.Specification)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    //
    public async Task<BusinessService> CreateService(BusinessService businessService)
    {
        _context.BusinessServices.Add(businessService);
        await _context.SaveChangesAsync();

        return businessService;
    }

    public async Task<BusinessService> UpdateService(BusinessService businessService)
    {
        var existingService = await GetServiceById(businessService.Id);
        _context.BusinessServices.Update(existingService);
        await _context.SaveChangesAsync();
        return existingService;
    }
    //
    public async Task<BusinessService> DeleteService(int id)
    {
        var existingService = await GetServiceById(id);
        _context.BusinessServices.Remove(existingService);
        await _context.SaveChangesAsync();
        return existingService;
    }

    public async Task<IEnumerable<BusinessService>> GetServicesBySpecification(int specificationId)
    {
        return await GetQueryableAsync()
            .Include(p => p.Specification)
            .Where(p => p.Specification.Id == specificationId)
            .ToListAsync();
    }
}