using EF_Core_Assignment_1.Core.Entities;

namespace EFCoreAssignment.Domain.Entities;

public class ProjectEmployee : BaseEntity
{
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    
    public bool Enable { get; set; }
}