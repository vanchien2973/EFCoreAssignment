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
        if (salary == null)
        {
            throw new ArgumentNullException(nameof(salary), "Salary cannot be null.");
        }
        await _context.Salaries.AddAsync(salary);
    }

    public async Task DeleteAsync(Guid id)
    {
        var salary = await GetByIdAsync(id);
        if (salary == null)
        {
            throw new KeyNotFoundException($"Salary with id {id} not found.");
        }
        _context.Salaries.Remove(salary);
    }

    public async Task<IEnumerable<Salary>> GetAllAsync()
    {
        return await _context.Salaries
            .Include(s => s.Employee)
            .ToListAsync();
    }

    public async Task<Salary?> GetByIdAsync(Guid id)
    {
        return await _context.Salaries
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Salary salary)
    {
        if (salary == null)
        {
            throw new ArgumentNullException(nameof(salary), "Salary cannot be null.");
        }
        _context.Salaries.Update(salary);
    }
}