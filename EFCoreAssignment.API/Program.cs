using EFCoreAssignment.Application.DTOs.Department;
using EFCoreAssignment.Application.DTOs.Employee;
using EFCoreAssignment.Application.DTOs.Project;
using EFCoreAssignment.Application.DTOs.ProjectEmployee;
using EFCoreAssignment.Application.DTOs.Salary;
using EFCoreAssignment.Application.Interfaces;
using EFCoreAssignment.Application.Services;
using EFCoreAssignment.Application.Validators.Department;
using EFCoreAssignment.Application.Validators.Employee;
using EFCoreAssignment.Application.Validators.Project;
using EFCoreAssignment.Application.Validators.ProjectEmployee;
using EFCoreAssignment.Application.Validators.Salary;
using EFCoreAssignment.Domain.Interfaces;
using EFCoreAssignment.Persistence;
using EFCoreAssignment.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using EFCoreAssignment.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Dependency Injection for repositories
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();
builder.Services.AddScoped<ISalaryRepository, SalaryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Dependency Injection for services
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IValidator<CreateDepartmentDto>, CreateDepartmentDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateDepartmentDto>, UpdateDepartmentDtoValidator>();
builder.Services.AddScoped<IValidator<CreateEmployeeDto>, CreateEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeDto>, UpdateEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<CreateProjectDto>, CreateProjectDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectDto>, UpdateProjectDtoValidator>();
builder.Services.AddScoped<IValidator<CreateProjectEmployeeDto>, CreateProjectEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateProjectEmployeeDto>, UpdateProjectEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<CreateSalaryDto>, CreateSalaryDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateSalaryDto>, UpdateSalaryDtoValidator>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectEmployeeService, ProjectEmployeeService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        DataSeeder.SeedDepartments(context);
        DataSeeder.SeedEmployees(context);
        DataSeeder.SeedProjects(context);
        DataSeeder.SeedSalaries(context);
        DataSeeder.SeedProjectEmployees(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Add custom middlewares
app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseRequestResponseLoggingMiddleware();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();