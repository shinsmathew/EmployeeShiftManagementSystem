using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Application.Features.Shift.Queries
{
    public class GetAverageHoursWorkedQuery : IRequest<double>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetAverageHoursWorkedQueryHandler : IRequestHandler<GetAverageHoursWorkedQuery, double>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IValidator<GetAverageHoursWorkedQuery> _validator;

        public GetAverageHoursWorkedQueryHandler(
            IShiftRepository shiftRepository,
            IValidator<GetAverageHoursWorkedQuery> validator)
        {
            _shiftRepository = shiftRepository;
            _validator = validator;
        }

        public async Task<double> Handle(GetAverageHoursWorkedQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _shiftRepository.GetAverageHoursWorkedAsync(request.StartDate, request.EndDate);
        }
    }
}