using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Application.Features.Employee.Commands
{
    public class DeactivateEmployeeCommand : IRequest<EmployeeResponseDto>
    {
        public int Id { get; set; }
    }
    public class DeactivateEmployeeCommandHandler : IRequestHandler<DeactivateEmployeeCommand, EmployeeResponseDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public DeactivateEmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeResponseDto> Handle(DeactivateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with {request.Id} is Not Found.");
            }

            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }
    }
}
