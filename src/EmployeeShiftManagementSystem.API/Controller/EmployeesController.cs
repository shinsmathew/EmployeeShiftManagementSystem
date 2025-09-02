using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Application.Features.Employee.Commands;
using EmployeeShiftManagementSystem.Application.Features.Employee.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeShiftManagementSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

       
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetActiveEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (employees, totalCount) = await _mediator.Send(new GetActiveEmployeesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            return Ok(employees);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
            var employee = await _mediator.Send(new CreateEmployeeCommand { EmployeeCreateDto = employeeCreateDto });
            return CreatedAtAction(nameof(GetActiveEmployees), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> UpdateEmployee(int id, [FromBody] EmployeeUpdateDto employeeUpdateDto)
        {
            var employee = await _mediator.Send(new UpdateEmployeeCommand
            {
                Id = id,
                EmployeeUpdateDto = employeeUpdateDto
            });
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> DeactivateEmployee(int id)
        {
            var employee = await _mediator.Send(new DeactivateEmployeeCommand { Id = id });
            return Ok(employee);
        }
    }
}


