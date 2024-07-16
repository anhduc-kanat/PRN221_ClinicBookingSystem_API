﻿using ClinicBookingSystem_BusinessObject.IEntities;
using ClinicBookingSystem_DataAccessObject.IBaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject.BaseDAO;

public class BaseDAO<T> : IBaseDAO<T> where T : class, IBaseEntities
{
    private readonly ClinicBookingSystemContext _dbContext;
    public BaseDAO(ClinicBookingSystemContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IQueryable<T> GetQueryableAsync()
    {
        return _dbContext.Set<T>()
            .Where(entity => entity.IsActive == true && entity.IsDelete == false);
    }
    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>()
            .Where(entity => entity.IsActive == true && entity.IsDelete == false)
            .FirstOrDefaultAsync(entity => entity.Id == id);
    }
    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }
    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.Now;
        _dbContext.Entry(entity).State = EntityState.Modified;
        return entity;
    }
    public async Task<T> DeleteAsync(T entity)
    {
        entity.IsDelete = true;
        _dbContext.Entry(entity).State = EntityState.Modified;
        return entity;
    }
    public IQueryable<T> GetAllAsync()
    {
        return _dbContext.Set<T>()
            .Where(entity => entity.IsActive == true && entity.IsDelete == false)
            .AsQueryable();
    }
    public async Task<IEnumerable<T>> GetAllAsyncPagination(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<T>()
            .Where(entity => entity.IsActive == true && entity.IsDelete == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<int> CountAllAsync()
    {
        return await _dbContext.Set<T>()
            .Where(entity => entity.IsActive == true && entity.IsDelete == false)
            .CountAsync();
    }
}