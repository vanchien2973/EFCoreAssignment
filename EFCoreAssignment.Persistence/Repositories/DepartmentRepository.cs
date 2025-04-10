using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAssignment.Persistence.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _context;

    public DepartmentRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Department department)
    {
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department));
        }

        await _context.Departments.AddAsync(department);
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await GetByIdAsync(id);
        if (department == null)
        {
            throw new KeyNotFoundException($"Department with id {id} not found.");
        }

        _context.Departments.Remove(department);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.Employees)
                .ThenInclude(e => e.Salary)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Department> GetByIdAsync(Guid id)
    {
        return await _context.Departments
            .Include(d => d.Employees)
                .ThenInclude(e => e.Salary)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task UpdateAsync(Department department)
    {
        if (department == null)
        {
            throw new ArgumentNullException(nameof(department));
        }

        _context.Departments.Update(department);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Departments
            .AnyAsync(d => d.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Departments
            .AnyAsync(d => d.Name.ToLower() == name.ToLower());
    }
}