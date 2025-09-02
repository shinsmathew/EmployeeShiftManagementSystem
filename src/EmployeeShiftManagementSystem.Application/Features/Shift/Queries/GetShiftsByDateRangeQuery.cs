using AutoMapper;
using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Core.Exceptions;
using EmployeeShiftManagementSystem.Core.Interfaces;
using FluentValidation;
using MediatR;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Queries
{
    public class GetShiftsByDateRangeQuery : IRequest<(IEnumerable<ShiftResponseDto>, int)>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        
    }
    public class GetShiftsByDateRangeQueryHandler : IRequestHandler<GetShiftsByDateRangeQuery, (IEnumerable<ShiftResponseDto>, int)>
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<GetShiftsByDateRangeQuery> _validator;

        public GetShiftsByDateRangeQueryHandler(
            IShiftRepository shiftRepository,
            IMapper mapper,
            IValidator<GetShiftsByDateRangeQuery> validator)
        {
            _shiftRepository = shiftRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<(IEnumerable<ShiftResponseDto>, int)> Handle(GetShiftsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException($"Validation failed: {string.Join(", ", errors)}");
            }

            var shifts = await _shiftRepository.GetShiftsByDateRangeAsync(
                request.StartDate,
                request.EndDate,
                request.PageNumber,
                request.PageSize);

            var totalCount = await _shiftRepository.GetShiftsByDateRangeCountAsync(
                request.StartDate,
                request.EndDate);

            var shiftDtos = _mapper.Map<IEnumerable<ShiftResponseDto>>(shifts.Where(s => !s.IsDeleted));

            return (shiftDtos, totalCount);
        }
    }
}
    

