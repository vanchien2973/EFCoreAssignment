using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IValidator<CreateEmployeeDto> _createValidator;
    private readonly IValidator<UpdateEmployeeDto> _updateValidator;

    public EmployeesController(
        IEmployeeService employeeService,
        IValidator<CreateEmployeeDto> createValidator,
        IValidator<UpdateEmployeeDto> updateValidator)
    {
        _employeeService = employeeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDetailDto>> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            var employee = await _employeeService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateEmployeeDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        try
        {
            await _employeeService.UpdateAsync(updateDto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _employeeService.DeleteAsync(id);
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
    
    [HttpGet("with-departments")]
    public async Task<ActionResult<IEnumerable<EmployeeDepartmentDto>>> GetEmployeesWithDepartments()
    {
        var employees = await _employeeService.GetEmployeesWithDepartmentsAsync();
        return Ok(employees);
    }

    [HttpGet("with-projects")]
    public async Task<ActionResult<IEnumerable<EmployeeProjectsDto>>> GetEmployeesWithProjects()
    {
        var employees = await _employeeService.GetEmployeesWithProjectsAsync();
        return Ok(employees);
    }

    [HttpGet("high-earning-recent")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetHighEarningRecentEmployees()
    {
        var employees = await _employeeService.GetHighEarningRecentEmployeesAsync();
        return Ok(employees);
    }
}