using EFCoreAssignment.Application.DTOs.Project;

namespace EFCoreAssignment.Application.DTOs.Employee;

public class EmployeeDetailDto : EmployeeDto
{
    public string DepartmentName { get; set; }
    public IEnumerable<ProjectDto> Projects { get; set; }
}