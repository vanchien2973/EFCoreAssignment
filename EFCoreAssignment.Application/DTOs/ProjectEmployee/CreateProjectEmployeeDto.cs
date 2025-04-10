namespace EFCoreAssignment.Application.DTOs.ProjectEmployee;

public class CreateProjectEmployeeDto
{
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
    public bool Enable { get; set; } = true;
}
