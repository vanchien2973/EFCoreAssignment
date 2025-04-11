using EFCoreAssignment.Application.DTOs.ProjectEmployee;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

/// <summary>
/// Project employee assignment management controller
/// Exceptions are automatically handled by ExceptionHandlingMiddleware:
/// - KeyNotFoundException - Returns 404 Not Found
/// - ArgumentException, InvalidOperationException - Returns 400 Bad Request
/// - Other exceptions - Returns 500 Internal Server Error
/// </summary>
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

    /// <summary>
    /// Get all project employee assignments
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectEmployeeDto>>> GetAll()
    {
        var projectEmployees = await _projectEmployeeService.GetAllAsync();
        return Ok(projectEmployees);
    }

    /// <summary>
    /// Get project employee assignment by project ID and employee ID
    /// If not found, ExceptionHandlingMiddleware will catch KeyNotFoundException and return 404
    /// </summary>
    [HttpGet("{projectId:guid}/{employeeId:guid}")]
    public async Task<ActionResult<ProjectEmployeeDto>> GetById(Guid projectId, Guid employeeId)
    {
        var projectEmployee = await _projectEmployeeService.GetByIdAsync(projectId, employeeId);
        return Ok(projectEmployee);
    }

    /// <summary>
    /// Create a new project employee assignment
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProjectEmployeeDto>> Create(CreateProjectEmployeeDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var projectEmployee = await _projectEmployeeService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), 
            new { projectId = projectEmployee.ProjectId, employeeId = projectEmployee.EmployeeId }, 
            projectEmployee);
    }

    /// <summary>
    /// Update project employee assignment information
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateProjectEmployeeDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _projectEmployeeService.UpdateAsync(updateDto);
        return NoContent();
    }

    /// <summary>
    /// Delete a project employee assignment
    /// </summary>
    [HttpDelete("{projectId:guid}/{employeeId:guid}")]
    public async Task<IActionResult> Delete(Guid projectId, Guid employeeId)
    {
        await _projectEmployeeService.DeleteAsync(projectId, employeeId);
        return NoContent();
    }
}