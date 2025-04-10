namespace EFCoreAssignment.Application.DTOs.ProjectEmployee;

public class ProjectEmployeeDto
{
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
    public bool Enable { get; set; }
    public string ProjectName { get; set; }
    public string EmployeeName { get; set; }
}
