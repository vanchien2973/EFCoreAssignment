using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

/// <summary>
/// Employee salary management controller
/// Exceptions are automatically handled by ExceptionHandlingMiddleware:
/// - KeyNotFoundException - Returns 404 Not Found
/// - ArgumentException, InvalidOperationException - Returns 400 Bad Request
/// - Other exceptions - Returns 500 Internal Server Error
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalariesController : ControllerBase
{
    private readonly ISalaryService _salaryService;
    private readonly IValidator<CreateSalaryDto> _createValidator;
    private readonly IValidator<UpdateSalaryDto> _updateValidator;

    public SalariesController(
        ISalaryService salaryService,
        IValidator<CreateSalaryDto> createValidator,
        IValidator<UpdateSalaryDto> updateValidator)
    {
        _salaryService = salaryService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    /// <summary>
    /// Get all salary information
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalaryDto>>> GetAll()
    {
        var salaries = await _salaryService.GetAllAsync();
        return Ok(salaries);
    }

    /// <summary>
    /// Get salary information by ID
    /// If not found, ExceptionHandlingMiddleware will catch KeyNotFoundException and return 404
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SalaryDto>> GetById(Guid id)
    {
        var salary = await _salaryService.GetByIdAsync(id);
        return Ok(salary);
    }

    /// <summary>
    /// Create new salary information
    /// Errors such as employee not found are handled by ExceptionHandlingMiddleware
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SalaryDto>> Create(CreateSalaryDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var salary = await _salaryService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = salary.Id }, salary);
    }

    /// <summary>
    /// Update salary information
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateSalaryDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _salaryService.UpdateAsync(updateDto);
        return NoContent();
    }

    /// <summary>
    /// Delete salary information
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _salaryService.DeleteAsync(id);
        return NoContent();
    }
}