using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Project;

public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters");
    }
}