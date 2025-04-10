namespace EFCoreAssignment.Application.DTOs.Employee;

public class UpdateEmployeeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime JoinedDate { get; set; }
    public Guid DepartmentId { get; set; }
}
