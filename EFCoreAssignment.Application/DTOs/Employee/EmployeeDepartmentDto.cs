namespace EFCoreAssignment.Application.DTOs.Employee;

public class EmployeeDepartmentDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public DateTime JoinedDate { get; set; }
}