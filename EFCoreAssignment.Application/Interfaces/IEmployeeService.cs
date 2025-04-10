using EFCoreAssignment.Application.DTOs.Employee;

namespace EFCoreAssignment.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDetailDto> GetByIdAsync(Guid id);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
    Task UpdateAsync(UpdateEmployeeDto updateDto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<EmployeeDepartmentDto>> GetEmployeesWithDepartmentsAsync();
    Task<IEnumerable<EmployeeProjectsDto>> GetEmployeesWithProjectsAsync();
    Task<IEnumerable<EmployeeDto>> GetHighEarningRecentEmployeesAsync();
}