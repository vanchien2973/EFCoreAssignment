using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAssignment.Persistence.Repositories;

public class ProjectEmployeeRepository : IProjectEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectEmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ProjectEmployee projectEmployee)
    {
        if (projectEmployee == null)
        {
            throw new ArgumentNullException(nameof(projectEmployee), "ProjectEmployee cannot be null.");
        }
        await _context.ProjectEmployees.AddAsync(projectEmployee);
    }

    public async Task DeleteAsync(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await GetByIdAsync(projectId, employeeId);
        if (projectEmployee == null)
        {
            throw new KeyNotFoundException($"ProjectEmployee with ProjectId {projectId} and EmployeeId {employeeId} not found.");
        }
        _context.ProjectEmployees.Remove(projectEmployee);
    }

    public async Task<IEnumerable<ProjectEmployee>> GetAllAsync()
    {
        return await _context.ProjectEmployees
            .Include(pe => pe.Project)
            .Include(pe => pe.Employee)
            .ToListAsync();
    }

    public async Task<ProjectEmployee> GetByIdAsync(Guid projectId, Guid employeeId)
    {
        return await _context.ProjectEmployees
            .Include(pe => pe.Project)
            .Include(pe => pe.Employee)
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }

    public async Task UpdateAsync(ProjectEmployee projectEmployee)
    {
        if (projectEmployee == null)
        {
            throw new ArgumentNullException(nameof(projectEmployee), "ProjectEmployee cannot be null.");
        }
        _context.ProjectEmployees.Update(projectEmployee);
    }

    public async Task<bool> ExistsAsync(Guid projectId, Guid employeeId)
    {
        return await _context.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }
}