using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Queries
{
    public class GetTotalHoursWorkedQuery : IRequest<double>
    {
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

       
    }
    public class GetTotalHoursWorkedQueryHandler : IRequestHandler<GetTotalHoursWorkedQuery, double>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<GetTotalHoursWorkedQuery> _validator;

        public GetTotalHoursWorkedQueryHandler(
            IShiftRepository shiftRepository,
            IEmployeeRepository employeeRepository,
            IValidator<GetTotalHoursWorkedQuery> validator)
        {
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
            _validator = validator;
        }

        public async Task<double> Handle(GetTotalHoursWorkedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
            }

            // Check if employee exists
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with ID {request.EmployeeId} not found.");
            }

            return await _shiftRepository.GetTotalHoursWorkedAsync(
                request.EmployeeId,
                request.StartDate,
                request.EndDate);
        }
    }
}
    

