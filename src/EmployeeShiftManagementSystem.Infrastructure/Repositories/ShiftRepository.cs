using EmployeeShiftManagementSystem.Core.Entities;
using EmployeeShiftManagementSystem.Core.Interfaces;
using EmployeeShiftManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace EmployeeShiftManagementSystem.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Shift> GetByIdAsync(int id) 
        {
            return await _context.Shifts.AsNoTracking()
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        

        public async Task AddAsync(Shift entity)
        {
            await _context.Shifts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shift entity) 
        {
            _context.Shifts.Update(entity);
            await _context.SaveChangesAsync();
        }

        

        public async Task<bool> HasOverlappingShiftAsync(int employeeId, DateTime startTime, DateTime endTime, int? excludeShiftId = null)
        {
            var query = _context.Shifts
                .Where(s => s.EmployeeId == employeeId && !s.IsDeleted &&
                           ((startTime >= s.StartTime && startTime < s.EndTime) ||
                            (endTime > s.StartTime && endTime <= s.EndTime) ||
                            (startTime <= s.StartTime && endTime >= s.EndTime)));

            if (excludeShiftId.HasValue)
            {
                query = query.Where(s => s.Id != excludeShiftId.Value);
            }

            return await query.AnyAsync();
        }


        public async Task<IEnumerable<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => !s.IsDeleted &&
                           s.StartTime < endDate &&   
                           s.EndTime > startDate)     
                .OrderBy(s => s.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetShiftsByDateRangeCountAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Shifts
                .Where(s => !s.IsDeleted &&
                           s.StartTime < endDate &&
                           s.EndTime > startDate)
                .CountAsync();
        }

        public async Task<double> GetTotalHoursWorkedAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var shifts = await _context.Shifts
                .Where(s => s.EmployeeId == employeeId &&
                           !s.IsDeleted &&
                           s.StartTime < endDate &&
                           s.EndTime > startDate)
                .ToListAsync();

            return shifts.Sum(s => (s.EndTime - s.StartTime).TotalHours);
        }

        public async Task<Employee> GetEmployeeWithMostHoursAsync(DateTime startDate, DateTime endDate)
        {
            
            var query = _context.Shifts
                .Where(s => !s.IsDeleted &&
                           s.StartTime < endDate && 
                           s.EndTime > startDate)   
                .GroupBy(s => s.EmployeeId) 
                .Select(g => new
                {
                    EmployeeId = g.Key,
                    TotalHours = g.Sum(s => EF.Functions.DateDiffSecond(s.StartTime, s.EndTime) / 3600.0) // More precise calculation
                })
                .OrderByDescending(x => x.TotalHours);

           
            var topEmployeeStats = await query.FirstOrDefaultAsync();

            if (topEmployeeStats == null)
                return null;

         
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == topEmployeeStats.EmployeeId);
        }

        public async Task<double> GetAverageHoursWorkedAsync(DateTime startDate, DateTime endDate)
        {
            var result = await _context.Shifts
                .Where(s => !s.IsDeleted &&
                           s.StartTime < endDate &&
                           s.EndTime > startDate)
                .GroupBy(s => 1) // Group all records together
                .Select(g => new
                {
                    TotalHours = g.Sum(s => (double)(EF.Functions.DateDiffSecond(
                        s.StartTime > startDate ? s.StartTime : startDate,
                        s.EndTime < endDate ? s.EndTime : endDate
                    )) / 3600.0),
                    EmployeeCount = g.Select(s => s.EmployeeId).Distinct().Count()
                })
                .FirstOrDefaultAsync();

            return result?.EmployeeCount > 0 ? result.TotalHours / result.EmployeeCount : 0;
        }
    }
}
