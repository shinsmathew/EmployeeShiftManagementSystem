using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Application.Features.Shift.Commands;
using EmployeeShiftManagementSystem.Application.Features.Shift.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeShiftManagementSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShiftsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<ShiftResponseDto>>> GetShiftsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var (shifts, totalCount) = await _mediator.Send(new GetShiftsByDateRangeQuery
            {
                StartDate = startDate,
                EndDate = endDate,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            return Ok(shifts);
        }

        [HttpPost]
        public async Task<ActionResult<ShiftResponseDto>> CreateShift([FromBody] ShiftCreateDto shiftCreateDto)
        {
            var shift = await _mediator.Send(new CreateShiftCommand { ShiftCreateDto = shiftCreateDto });
            return shift;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShiftResponseDto>> UpdateShift(int id, [FromBody] ShiftUpdateDto shiftUpdateDto)
        {
            var shift = await _mediator.Send(new UpdateShiftCommand
            {
                Id = id,
                ShiftUpdateDto = shiftUpdateDto
            });
            return Ok(shift);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ShiftResponseDto>> DeleteShift(int id)
        {
            var shift = await _mediator.Send(new DeleteShiftCommand { Id = id });
            return Ok(shift);
        }

        [HttpGet("total-hours/{employeeId}")]
        public async Task<ActionResult<double>> GetTotalHoursWorked(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var totalHours = await _mediator.Send(new GetTotalHoursWorkedQuery
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate
            });
            return Ok(totalHours);
        }

        [HttpGet("most-hours")]
        public async Task<ActionResult> GetEmployeeWithMostHours([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var (employee, totalHours) = await _mediator.Send(new GetEmployeeWithMostHoursQuery
            {
                StartDate = startDate,
                EndDate = endDate
            });

            if (employee == null)
                return NotFound("No shifts found in the specified date range"); 

            return Ok(new { Employee = employee, TotalHours = totalHours });
        }

        [HttpGet("average-hours")]
        public async Task<ActionResult<double>> GetAverageHoursWorked([FromQuery] DateTime startDate,[FromQuery] DateTime endDate)
        {
            var averageHours = await _mediator.Send(new GetAverageHoursWorkedQuery
            {
                StartDate = startDate,
                EndDate = endDate
            });
            return Ok(averageHours);
        }


    }
}