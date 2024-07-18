using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_DataAccessObject.BaseDAO;
using ClinicBookingSystem_DataAcessObject.DBContext;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem_DataAccessObject;

public class SpecificationDAO : BaseDAO<Specification>
{
    private readonly ClinicBookingSystemContext _context;
    public SpecificationDAO(ClinicBookingSystemContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Specification>> GetAllSpecifications()
    {
        return await GetQueryableAsync()
            .ToListAsync();
    }
    //
    public async Task<Specification> GetSpecificationById(int id)
    {
        return await GetQueryableAsync()
            .Include(p => p.BusinessServices)
            .Include(p => p.Users)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    //
    public async Task<Specification> CreateSpecification(Specification specification)
    {
        _context.Specifications.Add(specification);
        await _context.SaveChangesAsync();

        return specification;
    }

    public async Task<Specification> UpdateSpecification(Specification specification)
    {
        var existingSpecification = await GetSpecificationById(specification.Id);
        _context.Specifications.Update(existingSpecification);
        await _context.SaveChangesAsync();
        return existingSpecification;
    }
    public async Task<Specification> DeleteSpecification(int id)
    {
        var existingSpecification = await GetSpecificationById(id);
        _context.Specifications.Remove(existingSpecification);
        await _context.SaveChangesAsync();
        return existingSpecification;
    }

}