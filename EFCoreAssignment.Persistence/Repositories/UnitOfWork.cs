using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private bool _disposed = false;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Departments = new DepartmentRepository(_context);
        Employees = new EmployeeRepository(_context);
        Projects = new ProjectRepository(_context);
        Salaries = new SalaryRepository(_context);
        ProjectEmployees = new ProjectEmployeeRepository(_context);
    }

    public IDepartmentRepository Departments { get; }
    public IEmployeeRepository Employees { get; }
    public IProjectRepository Projects { get; }
    public ISalaryRepository Salaries { get; }
    public IProjectEmployeeRepository ProjectEmployees { get; }

    public async Task<int> CommitAsync()
    {
        if (_context.Database.CurrentTransaction != null)
        {
            return await _context.SaveChangesAsync();
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var result = await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}