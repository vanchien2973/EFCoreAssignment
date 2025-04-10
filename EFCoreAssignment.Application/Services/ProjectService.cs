using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IUnitOfWork unitOfWork, IProjectRepository projectRepository)
    {
        _unitOfWork = unitOfWork;
        _projectRepository = projectRepository;
    }

    public async Task<ProjectDetailDto> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;

        return new ProjectDetailDto
        {
            Id = project.Id,
            Name = project.Name,
            Employees = project.ProjectEmployees?
                .Where(pe => pe.Enable)
                .Select(pe => new EmployeeDto
                {
                    Id = pe.Employee.Id,
                    Name = pe.Employee.Name,
                    JoinedDate = pe.Employee.JoinedDate,
                    DepartmentId = pe.Employee.DepartmentId,
                    SalaryAmount = pe.Employee.Salary?.Amount ?? 0
                }) ?? Enumerable.Empty<EmployeeDto>()
        };
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync();
        return projects.Select(p => new ProjectDto
        {
            Id = p.Id,
            Name = p.Name
        });
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto createDto)
    {
        if (await _projectRepository.NameExistsAsync(createDto.Name))
        {
            throw new InvalidOperationException("Project name already exists");
        }

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name
        };

        await _projectRepository.AddAsync(project);
        await _unitOfWork.CommitAsync();

        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name
        };
    }

    public async Task UpdateAsync(UpdateProjectDto updateDto)
    {
        var project = await _projectRepository.GetByIdAsync(updateDto.Id);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        // Check if name is changed and new name already exists
        if (!project.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase) && 
            await _projectRepository.NameExistsAsync(updateDto.Name))
        {
            throw new InvalidOperationException("Project name already exists");
        }

        project.Name = updateDto.Name;
        await _projectRepository.UpdateAsync(project);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        if (await HasActiveEmployeesAsync(id))
        {
            throw new InvalidOperationException("Cannot delete project with active employees");
        }

        await _projectRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _projectRepository.NameExistsAsync(name);
    }

    public async Task<bool> HasActiveEmployeesAsync(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project?.ProjectEmployees?.Any(pe => pe.Enable) ?? false;
    }
}