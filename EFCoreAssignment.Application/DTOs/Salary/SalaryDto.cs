namespace EFCoreAssignment.Application.DTOs.Salary;

public class SalaryDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public decimal Amount { get; set; }
}