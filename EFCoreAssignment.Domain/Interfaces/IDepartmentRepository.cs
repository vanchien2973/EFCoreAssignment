using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Domain.Interfaces;

public interface IDepartmentRepository
{
    Task<Department> GetByIdAsync(Guid id);
    Task<IEnumerable<Department>> GetAllAsync();
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(Guid id);
}