using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Application.Features.Shift.Commands
{
    public class DeleteShiftCommand : IRequest<ShiftResponseDto>
    {
        public int Id { get; set; }
    }

    public class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, ShiftResponseDto>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;

        public DeleteShiftCommandHandler(IShiftRepository shiftRepository, IMapper mapper)
        {
            _shiftRepository = shiftRepository;
            _mapper = mapper;
        }

        public async Task<ShiftResponseDto> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
        {
            var shift = await _shiftRepository.GetByIdAsync(request.Id);
            if (shift == null || shift.IsDeleted)
            {
                throw new NotFoundException($"Shift with ID {request.Id} not found.");
            }

            shift.IsDeleted = true;
            shift.UpdatedAt = DateTime.UtcNow;

            await _shiftRepository.UpdateAsync(shift);

            var shiftResponse = _mapper.Map<ShiftResponseDto>(shift);
            shiftResponse.EmployeeName = $"{shift.Employee.FirstName} {shift.Employee.LastName}";

            return shiftResponse;
        }
    }
}
