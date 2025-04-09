using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAssignment.Persistence.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            _context.Employees.Remove(employee);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> GetByIdAsync(Guid id)
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Salary)
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task UpdateAsync(Employee employee)
    {
        _context.Employees.Update(employee);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithDepartmentsAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesWithProjectsAsync()
    {
        return await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetHighEarningRecentEmployeesAsync()
    {
        var date = new DateTime(2024, 1, 1);
        return await _context.Employees
            .Include(e => e.Salary)
            .Where(e => e.Salary.Amount > 100 && e.JoinedDate >= date)
            .ToListAsync();
    }
}