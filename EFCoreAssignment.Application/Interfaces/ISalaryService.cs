using EFCoreAssignment.Application.DTOs.Salary;

namespace EFCoreAssignment.Application.Interfaces;

public interface ISalaryService
{
    Task<SalaryDto> GetByIdAsync(Guid id);
    Task<IEnumerable<SalaryDto>> GetAllAsync();
    Task<SalaryDto> CreateAsync(CreateSalaryDto createDto);
    Task UpdateAsync(UpdateSalaryDto updateDto);
    Task DeleteAsync(Guid id);
    Task<bool> EmployeeHasSalaryAsync(Guid employeeId);
}