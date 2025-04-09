using EF_Core_Assignment_1.Core.Entities;

namespace EFCoreAssignment.Domain.Entities;

public class Project : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ProjectEmployee> ProjectEmployees { get; set; }
}