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
        await _context.ProjectEmployees.AddAsync(projectEmployee);
    }

    public async Task DeleteAsync(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await GetByIdAsync(projectId, employeeId);
        if (projectEmployee != null)
        {
            _context.ProjectEmployees.Remove(projectEmployee);
        }
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
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);
    }

    public async Task UpdateAsync(ProjectEmployee projectEmployee)
    {
        _context.ProjectEmployees.Update(projectEmployee);
    }
}