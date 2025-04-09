using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Domain.Interfaces;

public interface IProjectEmployeeRepository
{
    Task<ProjectEmployee> GetByIdAsync(Guid projectId, Guid employeeId);
    Task<IEnumerable<ProjectEmployee>> GetAllAsync();
    Task AddAsync(ProjectEmployee projectEmployee);
    Task UpdateAsync(ProjectEmployee projectEmployee);
    Task DeleteAsync(Guid projectId, Guid employeeId);
}