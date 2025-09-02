using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Commands
{
    public class CreateShiftCommand : IRequest<ShiftResponseDto>
    {
        public ShiftCreateDto ShiftCreateDto { get; set; } = new ShiftCreateDto();
    }

    public class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, ShiftResponseDto>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ShiftCreateDto> _validator;

        public CreateShiftCommandHandler(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository, IMapper mapper, IValidator<ShiftCreateDto> validator)
        {
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
        }   

        public async Task<ShiftResponseDto> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
          
            var validationResult = await _validator.ValidateAsync(request.ShiftCreateDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            
            var employee = await _employeeRepository.GetByIdAsync(request.ShiftCreateDto.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {request.ShiftCreateDto.EmployeeId} not found.");
            }

            
            if (!employee.IsActive)
            {
                throw new BadRequestException("Cannot assign shift to an inactive employee.");
            }

            var hasOverlap = await _shiftRepository.HasOverlappingShiftAsync(
                request.ShiftCreateDto.EmployeeId,
                request.ShiftCreateDto.StartTime,
                request.ShiftCreateDto.EndTime);

            if (hasOverlap)
            {
                throw new OverlappingShiftException("Employee already has a shift during this time period.");
            }

            var shift = _mapper.Map<Core.Entities.Shift>(request.ShiftCreateDto);
            shift.IsDeleted = false;

            await _shiftRepository.AddAsync(shift);

            var shiftResponse = _mapper.Map<ShiftResponseDto>(shift);
            shiftResponse.EmployeeName = $"{employee.FirstName} {employee.LastName}";

            return shiftResponse;
        }

    }
}
