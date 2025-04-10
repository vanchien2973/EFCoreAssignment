using EFCoreAssignment.Application.DTOs.ProjectEmployee;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectEmployeesController : ControllerBase
{
    private readonly IProjectEmployeeService _projectEmployeeService;
    private readonly IValidator<CreateProjectEmployeeDto> _createValidator;
    private readonly IValidator<UpdateProjectEmployeeDto> _updateValidator;

    public ProjectEmployeesController(
        IProjectEmployeeService projectEmployeeService,
        IValidator<CreateProjectEmployeeDto> createValidator,
        IValidator<UpdateProjectEmployeeDto> updateValidator)
    {
        _projectEmployeeService = projectEmployeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectEmployeeDto>>> GetAll()
    {
        var projectEmployees = await _projectEmployeeService.GetAllAsync();
        return Ok(projectEmployees);
    }

    [HttpGet("{projectId}/{employeeId}")]
    public async Task<ActionResult<ProjectEmployeeDto>> GetById(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await _projectEmployeeService.GetByIdAsync(projectId, employeeId);
        if (projectEmployee == null) return NotFound();
        return Ok(projectEmployee);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectEmployeeDto>> Create(CreateProjectEmployeeDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var projectEmployee = await _projectEmployeeService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), 
                new { projectId = projectEmployee.ProjectId, employeeId = projectEmployee.EmployeeId }, 
                projectEmployee);
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

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectEmployeeDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _projectEmployeeService.UpdateAsync(updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{projectId}/{employeeId}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid employeeId)
    {
        try
        {
            await _projectEmployeeService.DeleteAsync(projectId, employeeId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}