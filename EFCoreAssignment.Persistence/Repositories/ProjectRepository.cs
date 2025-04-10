using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAssignment.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Project project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");
        }
        await _context.Projects.AddAsync(project);
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await GetByIdAsync(id);
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project), "Project not found.");
        }
        _context.Projects.Remove(project);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects.ToListAsync();
    }

    public async Task<Project> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.ProjectEmployees)
            .ThenInclude(pe => pe.Employee)
                .ThenInclude(e => e.Salary)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Project project)
    {
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project), "Project cannot be null.");
        }
        _context.Projects.Update(project);
    }
    
    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Projects
            .AnyAsync(p => p.Name.ToLower() == name.ToLower());
    }
}