using System.ComponentModel.DataAnnotations;

namespace EFCoreAssignment.Application.DTOs.Department;

public class UpdateDepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}