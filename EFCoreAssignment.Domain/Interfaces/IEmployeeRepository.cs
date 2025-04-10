using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee> GetByIdAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<Employee>> GetAllEmployeesWithDepartmentsAsync();
    Task<IEnumerable<Employee>> GetAllEmployeesWithProjectsAsync();
    Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(DateTime fromDate, decimal minSalary);
}