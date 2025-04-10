using EFCoreAssignment.Application.DTOs.Department;
using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IUnitOfWork unitOfWork, IDepartmentRepository departmentRepository)
    {
        _unitOfWork = unitOfWork;
        _departmentRepository = departmentRepository;
    }

    public async Task<DepartmentDetailDto> GetByIdAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null) return null;

        return new DepartmentDetailDto
        {
            Id = department.Id,
            Name = department.Name,
            Employees = department.Employees?.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                JoinedDate = e.JoinedDate,
                DepartmentId = e.DepartmentId,
                SalaryAmount = e.Salary?.Amount ?? 0
            }) ?? Enumerable.Empty<EmployeeDto>()
        };
    }

    public async Task<IEnumerable<DepartmentDetailDto>> GetAllAsync()
    {
        var departments = await _departmentRepository.GetAllAsync();
        return departments.Select(d => new DepartmentDetailDto
        {
            Id = d.Id,
            Name = d.Name,
            Employees = d.Employees?.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                JoinedDate = e.JoinedDate,
                DepartmentId = e.DepartmentId,
                SalaryAmount = e.Salary?.Amount ?? 0
            }) ?? Enumerable.Empty<EmployeeDto>()
        });
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto)
    {
        if (await _departmentRepository.NameExistsAsync(createDto.Name))
        {
            throw new InvalidOperationException("Department name already exists");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name
        };

        await _departmentRepository.AddAsync(department);
        await _unitOfWork.CommitAsync();

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
        };
    }

    public async Task UpdateAsync(UpdateDepartmentDto updateDto)
    {
        var department = await _departmentRepository.GetByIdAsync(updateDto.Id);
        if (department == null)
        {
            throw new KeyNotFoundException("Department not found");
        }
        
        if (!department.Name.Equals(updateDto.Name, StringComparison.OrdinalIgnoreCase) && 
            await _departmentRepository.NameExistsAsync(updateDto.Name))
        {
            throw new InvalidOperationException("Department name already exists");
        }

        department.Name = updateDto.Name;
        await _departmentRepository.UpdateAsync(department);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);
        if (department == null)
        {
            throw new KeyNotFoundException("Department not found");
        }

        if (await HasEmployeesAsync(id))
        {
            throw new InvalidOperationException("Cannot delete department with employees");
        }

        await _departmentRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _departmentRepository.NameExistsAsync(name);
    }

    public async Task<bool> HasEmployeesAsync(Guid departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        return department?.Employees?.Any() ?? false;
    }
}