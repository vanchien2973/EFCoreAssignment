using EF_Core_Assignment_1.Core.Entities;

namespace EFCoreAssignment.Domain.Entities;

public class Salary : BaseEntity
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
}