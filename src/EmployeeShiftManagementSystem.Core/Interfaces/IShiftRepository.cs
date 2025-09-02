using EmployeeShiftManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IShiftRepository : IRepository<Shift>
    {
        Task<bool> HasOverlappingShiftAsync(int employeeId, DateTime startTime, DateTime endTime, int? excludeShiftId = null);
        Task<IEnumerable<Shift>> GetShiftsByEmployeeIdAsync(int employeeId, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize);
        Task<int> GetShiftsByDateRangeCountAsync(DateTime startDate, DateTime endDate);
        Task<double> GetTotalHoursWorkedAsync(int employeeId, DateTime startDate, DateTime endDate);
        Task<Employee> GetEmployeeWithMostHoursAsync(DateTime startDate, DateTime endDate);
        Task<double> GetAverageHoursWorkedAsync(DateTime startDate, DateTime endDate);
    }
}
