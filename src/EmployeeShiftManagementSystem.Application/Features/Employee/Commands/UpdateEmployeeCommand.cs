using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Application.Features.Employee.Validators;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Application.Features.Employee.Commands
{
    public class UpdateEmployeeCommand : IRequest<EmployeeResponseDto>
    {
        public int Id { get; set; }
        public EmployeeUpdateDto EmployeeUpdateDto { get; set; } = new EmployeeUpdateDto();
    }

    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeResponseDto> 
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<EmployeeUpdateDto> _validator;

        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper, IValidator<EmployeeUpdateDto> validator)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _validator = validator;
        }
        public async Task<EmployeeResponseDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var ValidatorResult = await _validator.ValidateAsync(request.EmployeeUpdateDto, cancellationToken);
            if (!ValidatorResult.IsValid)
            {
                throw new ValidationException(ValidatorResult.Errors);
            }

            var employee = await _employeeRepository.GetByIdAsync(request.Id);

            if (employee == null)
            {
                throw new NotFoundException($"Employee with Id {request.Id} not found.");
            }

            var UpdatedEmployee = _mapper.Map(request.EmployeeUpdateDto, employee);
            UpdatedEmployee.UpdatedAt = DateTime.UtcNow;
            await _employeeRepository.UpdateAsync(UpdatedEmployee);
            var employeeResponseDto = _mapper.Map<EmployeeResponseDto>(UpdatedEmployee);
            return employeeResponseDto;


        }

    }
}
