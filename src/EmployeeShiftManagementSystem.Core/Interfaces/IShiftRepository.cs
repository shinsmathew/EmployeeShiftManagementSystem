using EmployeeShiftManagementSystem.Core.Entities;


namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<bool> HasOverlappingShiftAsync(int employeeId, DateTime startTime, DateTime endTime, int? excludeShiftId = null);
        Task<IEnumerable<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize);
        Task<int> GetShiftsByDateRangeCountAsync(DateTime startDate, DateTime endDate);
        Task<double> GetTotalHoursWorkedAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<Employee> GetEmployeeWithMostHoursAsync(DateTime startDate, DateTime endDate);
    }
}
