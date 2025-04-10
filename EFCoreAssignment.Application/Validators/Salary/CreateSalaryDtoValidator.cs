using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Salary;

public class CreateSalaryDtoValidator : AbstractValidator<CreateSalaryDto>
{
    public CreateSalaryDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required")
            .WithMessage("Employee does not exist");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary amount must be greater than 0");
    }
}