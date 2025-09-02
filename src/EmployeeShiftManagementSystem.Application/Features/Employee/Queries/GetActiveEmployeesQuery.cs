using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Employee;
using EmployeeShiftManagementSystem.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Application.Features.Employee.Queries
{
    public class GetActiveEmployeesQuery : IRequest<(IEnumerable<EmployeeResponseDto>, int)>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetActiveEmployeesQueryHandler : IRequestHandler<GetActiveEmployeesQuery, (IEnumerable<EmployeeResponseDto>, int)>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetActiveEmployeesQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<EmployeeResponseDto>, int)> Handle(GetActiveEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetActiveEmployeesAsync(request.PageNumber, request.PageSize);
            var totalCount = await _employeeRepository.GetActiveEmployeesCountAsync();

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);

            return (employeeDtos, totalCount);
        }
    }
}
