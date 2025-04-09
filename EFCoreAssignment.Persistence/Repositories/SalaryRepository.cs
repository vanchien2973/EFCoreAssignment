using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAssignment.Persistence.Repositories;

public class SalaryRepository : ISalaryRepository
{
    private readonly ApplicationDbContext _context;

    public SalaryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Salary salary)
    {
        await _context.Salaries.AddAsync(salary);
    }

    public async Task DeleteAsync(Guid id)
    {
        var salary = await GetByIdAsync(id);
        if (salary != null)
        {
            _context.Salaries.Remove(salary);
        }
    }

    public async Task<IEnumerable<Salary>> GetAllAsync()
    {
        return await _context.Salaries.ToListAsync();
    }

    public async Task<Salary> GetByIdAsync(Guid id)
    {
        return await _context.Salaries.FindAsync(id);
    }

    public async Task UpdateAsync(Salary salary)
    {
        _context.Salaries.Update(salary);
    }
}