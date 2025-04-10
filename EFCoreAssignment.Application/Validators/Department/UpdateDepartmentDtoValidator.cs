using EFCoreAssignment.Application.DTOs.Department;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Department;

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator(IDepartmentService departmentService)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Department ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");
    }
}