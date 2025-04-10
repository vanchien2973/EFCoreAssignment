using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Domain.Entities;
using EFCoreAssignment.Domain.Interfaces;

namespace EFCoreAssignment.Application.Services;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISalaryRepository _salaryRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public SalaryService(
        IUnitOfWork unitOfWork,
        ISalaryRepository salaryRepository,
        IEmployeeRepository employeeRepository)
    {
        _unitOfWork = unitOfWork;
        _salaryRepository = salaryRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<SalaryDto> GetByIdAsync(Guid id)
    {
        var salary = await _salaryRepository.GetByIdAsync(id);
        if (salary == null) return null;

        var employee = await _employeeRepository.GetByIdAsync(salary.EmployeeId);

        return new SalaryDto
        {
            Id = salary.Id,
            EmployeeId = salary.EmployeeId,
            EmployeeName = employee?.Name ?? "Unknown",
            Amount = salary.Amount
        };
    }

    public async Task<IEnumerable<SalaryDto>> GetAllAsync()
    {
        var salaries = await _salaryRepository.GetAllAsync();
        var salaryDtos = new List<SalaryDto>();

        foreach (var salary in salaries)
        {
            var employee = await _employeeRepository.GetByIdAsync(salary.EmployeeId);
            salaryDtos.Add(new SalaryDto
            {
                Id = salary.Id,
                EmployeeId = salary.EmployeeId,
                EmployeeName = employee?.Name ?? "Unknown",
                Amount = salary.Amount
            });
        }

        return salaryDtos;
    }

    public async Task<SalaryDto> CreateAsync(CreateSalaryDto createDto)
    {
        // Check if employee exists
        var employee = await _employeeRepository.GetByIdAsync(createDto.EmployeeId);
        if (employee == null)
        {
            throw new KeyNotFoundException("Employee not found");
        }
        
        if (await EmployeeHasSalaryAsync(createDto.EmployeeId))
        {
            throw new InvalidOperationException("Employee already has a salary record");
        }
        
        if (createDto.Amount <= 0)
        {
            throw new ArgumentException("Salary amount must be greater than 0");
        }

        var salary = new Salary
        {
            Id = Guid.NewGuid(),
            EmployeeId = createDto.EmployeeId,
            Amount = createDto.Amount
        };

        await _salaryRepository.AddAsync(salary);
        await _unitOfWork.CommitAsync();

        return new SalaryDto
        {
            Id = salary.Id,
            EmployeeId = salary.EmployeeId,
            EmployeeName = employee.Name,
            Amount = salary.Amount
        };
    }

    public async Task UpdateAsync(UpdateSalaryDto updateDto)
    {
        var salary = await _salaryRepository.GetByIdAsync(updateDto.Id);
        if (salary == null)
        {
            throw new KeyNotFoundException("Salary not found");
        }
        
        if (updateDto.Amount <= 0)
        {
            throw new ArgumentException("Salary amount must be greater than 0");
        }

        salary.Amount = updateDto.Amount;
        await _salaryRepository.UpdateAsync(salary);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var salary = await _salaryRepository.GetByIdAsync(id);
        if (salary == null)
        {
            throw new KeyNotFoundException("Salary not found");
        }

        await _salaryRepository.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }

    public async Task<bool> EmployeeHasSalaryAsync(Guid employeeId)
    {
        var salaries = await _salaryRepository.GetAllAsync();
        return salaries.Any(s => s.EmployeeId == employeeId);
    }
}