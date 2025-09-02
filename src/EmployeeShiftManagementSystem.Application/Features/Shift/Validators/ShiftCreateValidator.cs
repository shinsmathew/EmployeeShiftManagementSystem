using EmployeeShiftManagementSystem.Application.DTOs.Shift;
using EmployeeShiftManagementSystem.Application.Features.Shift.Commands;
using EmployeeShiftManagementSystem.Application.Features.Shift.Queries;
using FluentValidation;


namespace EmployeeShiftManagementSystem.Application.Features.Shift.Validators
{
    public class ShiftCreateValidator: AbstractValidator<ShiftCreateDto>
    {
        public ShiftCreateValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");
        }
    }


    public class ShiftUpdateValidator : AbstractValidator<ShiftUpdateDto>
    {
        public ShiftUpdateValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");
        }
    }

    public class GetTotalHoursWorkedQueryValidator : AbstractValidator<GetTotalHoursWorkedQuery>
    {
        public GetTotalHoursWorkedQueryValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than 0.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");
        }
    }
    public class GetEmployeeWithMostHoursQueryValidator : AbstractValidator<GetEmployeeWithMostHoursQuery>
    {
        public GetEmployeeWithMostHoursQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");
        }
    }

    public class GetShiftsByDateRangeQueryValidator : AbstractValidator<GetShiftsByDateRangeQuery>
    {
        public GetShiftsByDateRangeQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }

    public class GetAverageHoursWorkedQueryValidator : AbstractValidator<GetAverageHoursWorkedQuery>
    {
        public GetAverageHoursWorkedQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date.");
        }
    }
}

