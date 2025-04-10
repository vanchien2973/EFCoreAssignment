using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Salary;

public class UpdateSalaryDtoValidator : AbstractValidator<UpdateSalaryDto>
{
    public UpdateSalaryDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Salary ID is required")
            .WithMessage("Salary does not exist");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Salary amount must be greater than 0");
    }
}