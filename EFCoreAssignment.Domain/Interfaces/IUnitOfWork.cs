namespace EFCoreAssignment.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDepartmentRepository Departments { get; }
    IEmployeeRepository Employees { get; }
    IProjectRepository Projects { get; }
    ISalaryRepository Salaries { get; }
    IProjectEmployeeRepository ProjectEmployees { get; }
    Task<int> CommitAsync();
}