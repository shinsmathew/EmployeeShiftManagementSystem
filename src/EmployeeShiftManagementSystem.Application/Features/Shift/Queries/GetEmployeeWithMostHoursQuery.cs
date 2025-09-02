using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Queries
{
    public class GetEmployeeWithMostHoursQuery : IRequest<(EmployeeResponseDto, double)>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetEmployeeWithMostHoursQueryHandler : IRequestHandler<GetEmployeeWithMostHoursQuery, (EmployeeResponseDto, double)>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetEmployeeWithMostHoursQuery> _validator;

        public GetEmployeeWithMostHoursQueryHandler(
            IShiftRepository shiftRepository,
            IMapper mapper,
            IValidator<GetEmployeeWithMostHoursQuery> validator)
        {
            _shiftRepository = shiftRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<(EmployeeResponseDto, double)> Handle(GetEmployeeWithMostHoursQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var employee = await _shiftRepository.GetEmployeeWithMostHoursAsync(request.StartDate, request.EndDate);

            if (employee == null)
                return (null, 0);

            var totalHours = await _shiftRepository.GetTotalHoursWorkedAsync(employee.Id, request.StartDate, request.EndDate);

            var employeeDto = _mapper.Map<EmployeeResponseDto>(employee);

            return (employeeDto, totalHours);
        }
    }
}

