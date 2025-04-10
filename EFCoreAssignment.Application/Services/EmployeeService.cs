using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeeService(IUnitOfWork unitOfWork, IDepartmentRepository departmentRepository)
    {
        _unitOfWork = unitOfWork;
        _departmentRepository = departmentRepository;
    }

    public async Task<EmployeeDetailDto> GetByIdAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null) return null;

        return new EmployeeDetailDto
        {
            Id = employee.Id,
            Name = employee.Name,
            JoinedDate = employee.JoinedDate,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department?.Name,
            SalaryAmount = employee.Salary?.Amount ?? 0,
            Projects = employee.ProjectEmployees?
                .Where(pe => pe.Enable)
                .Select(pe => new ProjectDto
                {
                    Id = pe.Project.Id,
                    Name = pe.Project.Name
                }) ?? Enumerable.Empty<ProjectDto>()
        };
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            JoinedDate = e.JoinedDate,
            DepartmentId = e.DepartmentId,
            SalaryAmount = e.Salary?.Amount ?? 0
        });
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
    {
        var department = await _departmentRepository.GetByIdAsync(createDto.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException("Department not found");
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            JoinedDate = createDto.JoinedDate,
            DepartmentId = createDto.DepartmentId
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.CommitAsync();

        return new EmployeeDto
        {
            Id = employee.Id,
            Name = employee.Name,
            JoinedDate = employee.JoinedDate,
            DepartmentId = employee.DepartmentId,
            SalaryAmount = 0
        };
    }

    public async Task UpdateAsync(UpdateEmployeeDto updateDto)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(updateDto.Id);
        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found");
        }
        
        var department = await _departmentRepository.GetByIdAsync(updateDto.DepartmentId);
        if (department == null)
        {
            throw new KeyNotFoundException("Department not found");
        }

        employee.Name = updateDto.Name;
        employee.JoinedDate = updateDto.JoinedDate;
        employee.DepartmentId = updateDto.DepartmentId;

        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found");
        }
        
        if (employee.ProjectEmployees.Any(pe => pe.Enable))
        {
            throw new InvalidOperationException("Cannot delete employee with active projects");
        }

        await _unitOfWork.Employees.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }
    
    public async Task<IEnumerable<EmployeeDepartmentDto>> GetEmployeesWithDepartmentsAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllEmployeesWithDepartmentsAsync();
        return employees.Select(e => new EmployeeDepartmentDto
        {
            EmployeeId = e.Id,
            EmployeeName = e.Name,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            JoinedDate = e.JoinedDate
        });
    }

    public async Task<IEnumerable<EmployeeProjectsDto>> GetEmployeesWithProjectsAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllEmployeesWithProjectsAsync();
        return employees.SelectMany(e => 
            e.ProjectEmployees?.Any() == true 
                ? e.ProjectEmployees.Select(pe => new EmployeeProjectsDto
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.Name,
                    ProjectId = pe.ProjectId,
                    ProjectName = pe.Project?.Name,
                    IsActive = pe.Enable
                })
                : new List<EmployeeProjectsDto> { new EmployeeProjectsDto
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.Name
                }}
        );
    }

    public async Task<IEnumerable<EmployeeDto>> GetHighEarningRecentEmployeesAsync()
    {
        var fromDate = new DateTime(2024, 1, 1);
        var minSalary = 100m;
        
        var employees = await _unitOfWork.Employees.GetFilteredEmployeesAsync(fromDate, minSalary);
        
        return employees.Select(e => new EmployeeDto
        {
            Id = e.Id,
            Name = e.Name,
            JoinedDate = e.JoinedDate,
            DepartmentId = e.DepartmentId,
            SalaryAmount = e.Salary?.Amount ?? 0
        });
    }
}