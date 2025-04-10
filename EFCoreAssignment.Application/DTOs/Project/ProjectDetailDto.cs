using EFCoreAssignment.Application.DTOs.Employee;

namespace EFCoreAssignment.Application.DTOs.Project;

public class ProjectDetailDto : ProjectDto
{
    public IEnumerable<EmployeeDto> Employees { get; set; }
}