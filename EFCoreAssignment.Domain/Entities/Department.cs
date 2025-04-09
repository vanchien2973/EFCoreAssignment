using EF_Core_Assignment_1.Core.Entities;

namespace EFCoreAssignment.Domain.Entities;

public class Department : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Employee> Employees { get; set; }
}