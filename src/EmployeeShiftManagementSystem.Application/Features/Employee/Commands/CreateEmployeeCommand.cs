using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Employee.Commands
{
    public class CreateEmployeeCommand : IRequest<EmployeeResponseDto>
    {
        public EmployeeCreateDto EmployeeCreateDto { get; set; } = new EmployeeCreateDto();
    }
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<EmployeeCreateDto> _validator;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository, 
            IValidator<EmployeeCreateDto> validator, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _validator = validator;
            _mapper = mapper;
        }


        public async Task<EmployeeResponseDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var ValidatorResult = await _validator.ValidateAsync(request.EmployeeCreateDto, cancellationToken);

            if (!ValidatorResult.IsValid)
            {
                throw new ValidationException(ValidatorResult.Errors);
            }

            bool isEmailExist = await _employeeRepository.IsEmailUniqueAsync(request.EmployeeCreateDto.Email);
            if (!isEmailExist)
            {
                throw new ApplicationException($"Email '{request.EmployeeCreateDto.Email}' is already in use.");
            }

            var employee = _mapper.Map<Core.Entities.Employee>(request.EmployeeCreateDto);

            employee.IsActive = true;

            await _employeeRepository.AddAsync(employee);

            var employeeResponseDto = _mapper.Map<EmployeeResponseDto>(employee);

            return employeeResponseDto;
        }
    }
}
