using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Commands
{
    public class UpdateShiftCommand : IRequest<ShiftResponseDto>
    {
        public int Id { get; set; }
        public ShiftUpdateDto ShiftUpdateDto { get; set; } = new ShiftUpdateDto();
    }

    public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, ShiftResponseDto>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ShiftUpdateDto> _validator;

        public UpdateShiftCommandHandler(
            IShiftRepository shiftRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper,
            IValidator<ShiftUpdateDto> validator)
        {
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<ShiftResponseDto> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            var ValidatorResult = await _validator.ValidateAsync(request.ShiftUpdateDto, cancellationToken);
            if (!ValidatorResult.IsValid)
            {
                throw new ValidationException(ValidatorResult.Errors);
            }

            var shift = await _shiftRepository.GetByIdAsync(request.Id);
            if (shift == null || shift.IsDeleted)
            { 

               throw new NotFoundException($"Shift with ID {request.Id} not found.");
            }

            var employee = await _employeeRepository.GetByIdAsync(request.ShiftUpdateDto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {request.ShiftUpdateDto.EmployeeId} not found.");
            }

            if (!employee.IsActive) 
            {
                throw new BadRequestException("Cannot assign shift to an inactive employee.");
            }
               

            var hasOverlap = await _shiftRepository.HasOverlappingShiftAsync(
                request.ShiftUpdateDto.EmployeeId,
                request.ShiftUpdateDto.StartTime,
                request.ShiftUpdateDto.EndTime,
                request.Id);

            if (hasOverlap)
            {
                throw new OverlappingShiftException("Employee already has a shift during this time period.");
            }

            _mapper.Map(request.ShiftUpdateDto, shift);
            shift.UpdatedAt = DateTime.UtcNow;

            await _shiftRepository.UpdateAsync(shift);

            var shiftResponse = _mapper.Map<ShiftResponseDto>(shift);
            shiftResponse.EmployeeName = $"{employee.FirstName} {employee.LastName}";

            return shiftResponse;
        }
    }
}
