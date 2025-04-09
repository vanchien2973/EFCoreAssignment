using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Employee>> GetEmployeesWithDepartmentsAsync();
    Task<IEnumerable<Employee>> GetEmployeesWithProjectsAsync();
    Task<IEnumerable<Employee>> GetHighEarningRecentEmployeesAsync();
}