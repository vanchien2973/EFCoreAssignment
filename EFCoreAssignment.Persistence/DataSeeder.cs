using EF_Core_Assignment_1.Core.Entities;
using EFCoreAssignment.Domain.Entities;

namespace EFCoreAssignment.Persistence;

public static class DataSeeder
{
    public static void SeedDepartments(ApplicationDbContext context)
    {
        if (!context.Departments.Any())
        {
            var now = DateTime.UtcNow;
            var departments = new List<Department>
            {
                new Department { Id = Guid.NewGuid(), Name = "Software Development", CreatedAt = now, UpdatedAt = now },
                new Department { Id = Guid.NewGuid(), Name = "Finance", CreatedAt = now, UpdatedAt = now },
                new Department { Id = Guid.NewGuid(), Name = "Accountant", CreatedAt = now, UpdatedAt = now },
                new Department { Id = Guid.NewGuid(), Name = "HR", CreatedAt = now, UpdatedAt = now }
            };
        
            context.Departments.AddRange(departments);
            context.SaveChanges();
        }
    }
}