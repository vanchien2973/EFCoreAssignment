using EFCoreAssignment.Application.DTOs.ProjectEmployee;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Application.Services;

public class ProjectEmployeeService : IProjectEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectEmployeeService(
        IUnitOfWork unitOfWork,
        IProjectEmployeeRepository projectEmployeeRepository,
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository)
    {
        _unitOfWork = unitOfWork;
        _projectEmployeeRepository = projectEmployeeRepository;
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<ProjectEmployeeDto> GetByIdAsync(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync(projectId, employeeId);
        if (projectEmployee == null) return null;

        return new ProjectEmployeeDto
        {
            ProjectId = projectEmployee.ProjectId,
            EmployeeId = projectEmployee.EmployeeId,
            Enable = projectEmployee.Enable,
            ProjectName = projectEmployee.Project?.Name,
            EmployeeName = projectEmployee.Employee?.Name
        };
    }

    public async Task<IEnumerable<ProjectEmployeeDto>> GetAllAsync()
    {
        var projectEmployees = await _projectEmployeeRepository.GetAllAsync();
        return projectEmployees.Select(pe => new ProjectEmployeeDto
        {
            ProjectId = pe.ProjectId,
            EmployeeId = pe.EmployeeId,
            Enable = pe.Enable,
            ProjectName = pe.Project?.Name,
            EmployeeName = pe.Employee?.Name
        });
    }

    public async Task<ProjectEmployeeDto> CreateAsync(CreateProjectEmployeeDto createDto)
    {
        // Check if project exists
        var project = await _projectRepository.GetByIdAsync(createDto.ProjectId);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found");
        }

        // Check if employee exists
        var employee = await _employeeRepository.GetByIdAsync(createDto.EmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found");
        }

        // Check if relationship already exists
        if (await _projectEmployeeRepository.ExistsAsync(createDto.ProjectId, createDto.EmployeeId))
        {
            throw new InvalidOperationException("Employee is already assigned to this project");
        }

        var projectEmployee = new ProjectEmployee
        {
            ProjectId = createDto.ProjectId,
            EmployeeId = createDto.EmployeeId,
            Enable = createDto.Enable
        };

        await _projectEmployeeRepository.AddAsync(projectEmployee);
        await _unitOfWork.CommitAsync();

        return new ProjectEmployeeDto
        {
            ProjectId = projectEmployee.ProjectId,
            EmployeeId = projectEmployee.EmployeeId,
            Enable = projectEmployee.Enable,
            ProjectName = project.Name,
            EmployeeName = employee.Name
        };
    }

    public async Task UpdateAsync(UpdateProjectEmployeeDto updateDto)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync(updateDto.ProjectId, updateDto.EmployeeId);
        if (projectEmployee == null)
        {
            throw new KeyNotFoundException("Project-Employee relationship not found");
        }

        projectEmployee.Enable = updateDto.Enable;
        await _projectEmployeeRepository.UpdateAsync(projectEmployee);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync(projectId, employeeId);
        if (projectEmployee == null)
        {
            throw new KeyNotFoundException("Project-Employee relationship not found");
        }

        await _projectEmployeeRepository.DeleteAsync(projectId, employeeId);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> ExistsAsync(Guid projectId, Guid employeeId)
    {
        return await _projectEmployeeRepository.ExistsAsync(projectId, employeeId);
    }
}