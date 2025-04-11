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
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        await _context.Employees.AddAsync(employee);
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with id {id} not found.");
        }

        _context.Employees.Remove(employee);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .Include(e => e.Salary)
            .ToListAsync();
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
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        _context.Employees.Update(employee);
    }
    
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Employees.AnyAsync(e => e.Id == id);
    }
    
    public async Task<IEnumerable<Employee>> GetAllEmployeesWithDepartmentsAsync()
    {
        return await _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Salary)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesWithProjectsAsync()
    {
        return await _context.Employees
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .Include(e => e.Salary)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(DateTime fromDate, decimal minSalary)
    {
        const string sql = """
                               SELECT e.* 
                               FROM Employees e
                               INNER JOIN Salaries s ON e.Id = s.EmployeeId
                               WHERE s.Amount > {0} AND e.JoinedDate >= {1}
                           """;

        return await _context.Employees
            .FromSqlRaw(sql, minSalary, fromDate)
            .Include(e => e.Department)
            .Include(e => e.Salary)
            .AsNoTracking()
            .ToListAsync();
    }
}