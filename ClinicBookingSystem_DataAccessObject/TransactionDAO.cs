﻿using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class TransactionDAO : BaseDAO<Transaction>
{
    private readonly ClinicBookingSystemContext _dbContext;
    public TransactionDAO(ClinicBookingSystemContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction> GetTransactionByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Appointment.Id == appointmentId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Transaction>> GetListTransactionByAppointmentId(int appointmentId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Appointment.Id == appointmentId)
            .ToListAsync();
    }
    public async Task<Transaction> GetTransactionByTransactionId(int transactionId)
    {
        return await GetQueryableAsync()
            .Where(p => p.Id == transactionId)
            .Include(p => p.Appointment)
            .FirstOrDefaultAsync();
    }
}