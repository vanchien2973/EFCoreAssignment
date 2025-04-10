using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalaryDto>>> GetAll()
    {
        var salaries = await _salaryService.GetAllAsync();
        return Ok(salaries);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SalaryDto>> GetById(Guid id)
    {
        var salary = await _salaryService.GetByIdAsync(id);
        if (salary == null) return NotFound();
        return Ok(salary);
    }

    [HttpPost]
    public async Task<ActionResult<SalaryDto>> Create(CreateSalaryDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var salary = await _salaryService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = salary.Id }, salary);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateSalaryDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _salaryService.UpdateAsync(updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _salaryService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}