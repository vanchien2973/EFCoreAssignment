using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Domain.Interfaces;

public interface ISalaryRepository
{
    Task<Salary> GetByIdAsync(Guid id);
    Task<IEnumerable<Salary>> GetAllAsync();
    Task AddAsync(Salary salary);
    Task UpdateAsync(Salary salary);
    Task DeleteAsync(Guid id);
}