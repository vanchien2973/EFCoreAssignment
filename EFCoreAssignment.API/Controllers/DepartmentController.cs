using EFCoreAssignment.Application.DTOs.Department;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentDetailDto>> GetById(Guid id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var department = await _departmentService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateDepartmentDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _departmentService.UpdateAsync(updateDto);
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
            await _departmentService.DeleteAsync(id);
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
