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


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly));


builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddValidatorsFromAssembly(typeof(EmployeeValidators).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ShiftCreateValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddHttpClient();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();