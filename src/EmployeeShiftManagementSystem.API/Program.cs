using EmployeeShiftManagementSystem.API.Middleware;
using EmployeeShiftManagementSystem.Application.Features.Employee.Commands;
using EmployeeShiftManagementSystem.Application.Features.Employee.Validators;
using EmployeeShiftManagementSystem.Application.Features.Shift.Validators;
using EmployeeShiftManagementSystem.Application.Mappings;
using EmployeeShiftManagementSystem.Core.Interfaces;
using EmployeeShiftManagementSystem.Infrastructure.Data;
using EmployeeShiftManagementSystem.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// FluentValidation Configuration (Equivalent to your extension method)
builder.Services.AddValidatorsFromAssembly(typeof(EmployeeValidators).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ShiftCreateValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// HttpClient
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();