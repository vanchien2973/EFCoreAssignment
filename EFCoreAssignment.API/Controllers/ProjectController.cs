using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IValidator<CreateProjectDto> _createValidator;
    private readonly IValidator<UpdateProjectDto> _updateValidator;

    public ProjectsController(
        IProjectService projectService,
        IValidator<CreateProjectDto> createValidator,
        IValidator<UpdateProjectDto> updateValidator)
    {
        _projectService = projectService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        var projects = await _projectService.GetAllAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDetailDto>> GetById(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var project = await _projectService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _projectService.UpdateAsync(updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _projectService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}