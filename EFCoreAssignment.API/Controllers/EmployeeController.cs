using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreAssignment.API.Controllers;

/// <summary>
/// Employee management controller
/// Exceptions are automatically handled by ExceptionHandlingMiddleware:
/// - KeyNotFoundException - Returns 404 Not Found
/// - ArgumentException, InvalidOperationException - Returns 400 Bad Request
/// - Other exceptions - Returns 500 Internal Server Error
/// </summary>
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

    /// <summary>
    /// Get all employees
    /// </summary>
    /// <returns>List of employees</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeService.GetAllAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get employee details by ID
    /// If not found, ExceptionHandlingMiddleware will catch KeyNotFoundException and return 404
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <returns>Employee detailed information</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeDetailDto>> GetById(Guid id)
    {
        var employee = await _employeeService.GetByIdAsync(id);
        return Ok(employee);
    }

    /// <summary>
    /// Create a new employee
    /// </summary>
    /// <param name="createDto">New employee information</param>
    /// <returns>Created employee</returns>
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var employee = await _employeeService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    /// <summary>
    /// Update employee information
    /// </summary>
    /// <param name="updateDto">Updated employee information</param>
    /// <returns>Processing result (204 No Content if successful)</returns>
    [HttpPut]
    public async Task<IActionResult> Update(UpdateEmployeeDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _employeeService.UpdateAsync(updateDto);
        return NoContent();
    }

    /// <summary>
    /// Delete an employee
    /// </summary>
    /// <param name="id">ID of the employee to delete</param>
    /// <returns>Processing result (204 No Content if successful)</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _employeeService.DeleteAsync(id);
        return NoContent();
    }
    
    /// <summary>
    /// Get list of employees with department information
    /// </summary>
    /// <returns>List of employees with their department information</returns>
    [HttpGet("with-departments")]
    public async Task<ActionResult<IEnumerable<EmployeeDepartmentDto>>> GetEmployeesWithDepartments()
    {
        var employees = await _employeeService.GetEmployeesWithDepartmentsAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get list of employees with project information
    /// </summary>
    /// <returns>List of employees with their project information</returns>
    [HttpGet("with-projects")]
    public async Task<ActionResult<IEnumerable<EmployeeProjectsDto>>> GetEmployeesWithProjects()
    {
        var employees = await _employeeService.GetEmployeesWithProjectsAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Get list of high-earning employees recently hired
    /// </summary>
    /// <returns>List of high-earning employees recently hired</returns>
    [HttpGet("high-earning-recent")]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetHighEarningRecentEmployees()
    {
        var employees = await _employeeService.GetHighEarningRecentEmployeesAsync();
        return Ok(employees);
    }
}