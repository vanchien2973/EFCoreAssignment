using EF_Core_Assignment_1.Core.Entities;

namespace EFCoreAssignment.Domain.Entities;

public class Employee : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime JoinedDate { get; set; }
    
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; }
    
    public Salary Salary { get; set; }
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
}