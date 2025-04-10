using EFCoreAssignment.Application.DTOs.Employee;

namespace EFCoreAssignment.Application.DTOs.Department;

public class DepartmentDetailDto : DepartmentDto
{
    public IEnumerable<EmployeeDto> Employees { get; set; }
}