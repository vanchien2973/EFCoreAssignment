using EFCoreAssignment.Application.DTOs.ProjectEmployee;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.ProjectEmployee;

public class UpdateProjectEmployeeDtoValidator : AbstractValidator<UpdateProjectEmployeeDto>
{
    public UpdateProjectEmployeeDtoValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("Project ID is required");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required");
        
    }
}