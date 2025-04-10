using EFCoreAssignment.Application.DTOs.ProjectEmployee;

namespace EFCoreAssignment.Application.Interfaces;

public interface IProjectEmployeeService
{
    Task<ProjectEmployeeDto> GetByIdAsync(Guid projectId, Guid employeeId);
    Task<IEnumerable<ProjectEmployeeDto>> GetAllAsync();
    Task<ProjectEmployeeDto> CreateAsync(CreateProjectEmployeeDto createDto);
    Task UpdateAsync(UpdateProjectEmployeeDto updateDto);
    Task DeleteAsync(Guid projectId, Guid employeeId);
    Task<bool> ExistsAsync(Guid projectId, Guid employeeId);
}