namespace EFCoreAssignment.Application.DTOs.Employee;

public class EmployeeProjectsDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public Guid? ProjectId { get; set; }
    public string ProjectName { get; set; }
    public bool IsActive { get; set; }
}