namespace EFCoreAssignment.Application.DTOs.Salary;

public class CreateSalaryDto
{
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
}