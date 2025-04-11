using EFCoreAssignment.Application.DTOs.Department;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

/// <summary>
/// Department management controller
/// Exceptions are automatically handled by ExceptionHandlingMiddleware:
/// - KeyNotFoundException - Returns 404 Not Found
/// - ArgumentException, InvalidOperationException - Returns 400 Bad Request
/// - Other exceptions - Returns 500 Internal Server Error
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IValidator<CreateDepartmentDto> _createValidator;
    private readonly IValidator<UpdateDepartmentDto> _updateValidator;

    public DepartmentsController(
        IDepartmentService departmentService,
        IValidator<CreateDepartmentDto> createValidator,
        IValidator<UpdateDepartmentDto> updateValidator)
    {
        _departmentService = departmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all departments
    /// </summary>
    /// <returns>List of departments</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    /// <summary>
    /// Get department by ID
    /// If not found, ExceptionHandlingMiddleware will catch KeyNotFoundException and return 404
    /// </summary>
    /// <param name="id">Department ID</param>
    /// <returns>Department detailed information</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentDetailDto>> GetById(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        return Ok(department); // ExceptionHandlingMiddleware will handle NotFound
    }

    /// <summary>
    /// Create a new department
    /// </summary>
    /// <param name="createDto">New department information</param>
    /// <returns>Created department</returns>
    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var department = await _departmentService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
    }

    /// <summary>
    /// Update department information
    /// </summary>
    /// <param name="updateDto">Updated department information</param>
    /// <returns>Processing result (204 No Content if successful)</returns>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateDepartmentDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _departmentService.UpdateAsync(updateDto);
        return NoContent();
    }

    /// <summary>
    /// Delete a department
    /// </summary>
    /// <param name="id">ID of the department to delete</param>
    /// <returns>Processing result (204 No Content if successful)</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _departmentService.DeleteAsync(id);
        return NoContent();
    }
}
