using EFCoreAssignment.Application.DTOs.Department;

namespace EFCoreAssignment.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDetailDto> GetByIdAsync(Guid id);
    Task<IEnumerable<DepartmentDetailDto>> GetAllAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto);
    Task UpdateAsync(UpdateDepartmentDto updateDto);
    Task DeleteAsync(Guid id);
    Task<bool> NameExistsAsync(string name);
}