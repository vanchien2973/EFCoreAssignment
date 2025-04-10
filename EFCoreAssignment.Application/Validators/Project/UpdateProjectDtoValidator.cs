using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;

namespace EFCoreAssignment.Application.Validators.Project;

public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    public UpdateProjectDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project ID is required")
            .WithMessage("Project does not exist");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required")
            .MaximumLength(100).WithMessage("Project name cannot exceed 100 characters");
    }
}