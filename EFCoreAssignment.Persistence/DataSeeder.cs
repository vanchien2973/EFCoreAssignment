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
    
    public static void SeedEmployees(ApplicationDbContext context)
    {
        if (!context.Employees.Any())
        {
            var now = DateTime.UtcNow;
            var employees = new List<Employee>
            {
                new Employee { Id = Guid.NewGuid(), Name = "John Doe", DepartmentId = context.Departments.First().Id, CreatedAt = now, UpdatedAt = now },
                new Employee { Id = Guid.NewGuid(), Name = "Jane Smith", DepartmentId = context.Departments.Skip(1).First().Id, CreatedAt = now, UpdatedAt = now }
            };
        
            context.Employees.AddRange(employees);
            context.SaveChanges();
        }
    }
    
    public static void SeedProjects(ApplicationDbContext context)
    {
        if (!context.Projects.Any())
        {
            var now = DateTime.UtcNow;
            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Project A", CreatedAt = now, UpdatedAt = now },
                new Project { Id = Guid.NewGuid(), Name = "Project B", CreatedAt = now, UpdatedAt = now }
            };
        
            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
    public static void SeedSalaries(ApplicationDbContext context)
    {
        if (!context.Salaries.Any())
        {
            var now = DateTime.UtcNow;
            var salaries = new List<Salary>
            {
                new Salary { Id = Guid.NewGuid(), Amount = 50000, EmployeeId = context.Employees.First().Id, CreatedAt = now, UpdatedAt = now },
                new Salary { Id = Guid.NewGuid(), Amount = 60000, EmployeeId = context.Employees.Skip(1).First().Id, CreatedAt = now, UpdatedAt = now }
            };
        
            context.Salaries.AddRange(salaries);
            context.SaveChanges();
        }
    }
    public static void SeedProjectEmployees(ApplicationDbContext context)
    {
        if (!context.ProjectEmployees.Any())
        {
            var now = DateTime.UtcNow;
            var projectEmployees = new List<ProjectEmployee>
            {
                new ProjectEmployee { Id = Guid.NewGuid(), ProjectId = context.Projects.First().Id, EmployeeId = context.Employees.First().Id, CreatedAt = now, UpdatedAt = now },
                new ProjectEmployee { Id = Guid.NewGuid(), ProjectId = context.Projects.Skip(1).First().Id, EmployeeId = context.Employees.Skip(1).First().Id, CreatedAt = now, UpdatedAt = now }
            };
        
            context.ProjectEmployees.AddRange(projectEmployees);
            context.SaveChanges();
        }
    }
}