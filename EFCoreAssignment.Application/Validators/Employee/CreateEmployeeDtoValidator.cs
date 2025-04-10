using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Domain.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Employee;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Employee name is required")
            .MaximumLength(100).WithMessage("Employee name cannot exceed 100 characters");

        RuleFor(x => x.JoinedDate)
            .NotEmpty().WithMessage("Joined date is required")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Joined date cannot be in the future");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department is required");

        RuleFor(x => x.SalaryAmount)
            .GreaterThan(0).WithMessage("Salary must be greater than 0");
    }
}