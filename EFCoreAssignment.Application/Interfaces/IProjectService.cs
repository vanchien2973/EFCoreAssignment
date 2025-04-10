using EFCoreAssignment.Application.DTOs.Project;

namespace EFCoreAssignment.Application.Interfaces;

public interface IProjectService
{
    Task<ProjectDetailDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto> CreateAsync(CreateProjectDto createDto);
    Task UpdateAsync(UpdateProjectDto updateDto);
    Task DeleteAsync(Guid id);
    Task<bool> NameExistsAsync(string name);
    Task<bool> HasActiveEmployeesAsync(Guid projectId);
}