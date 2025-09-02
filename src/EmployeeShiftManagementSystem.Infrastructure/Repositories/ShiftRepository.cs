using EmployeeShiftManagementSystem.Core.Entities;
using EmployeeShiftManagementSystem.Core.Interfaces;
using EmployeeShiftManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly ApplicationDbContext _context;

        public ShiftRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Shift> GetIdAsync(int id)
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Shift>> GetAllAsync()
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shift>> FindAsync(System.Linq.Expressions.Expression<Func<Shift, bool>> predicate)
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task AddAsync(Shift entity)
        {
            await _context.Shifts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Shift entity)
        {
            _context.Shifts.Update(entity);
            _context.SaveChanges();
        }

        public async Task Remove(Shift entity)
        {
            _context.Shifts.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Shifts.AnyAsync(s => s.Id == id);
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

        public async Task<IEnumerable<Shift>> GetShiftsByEmployeeIdAsync(int employeeId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Shifts
                .Include(s => s.Employee)
                .Where(s => s.EmployeeId == employeeId && !s.IsDeleted);

            if (startDate.HasValue)
            {
                query = query.Where(s => s.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(s => s.EndTime <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, int pageNumber, int pageSize)
        {
            return await _context.Shifts
                .Include(s => s.Employee)
                .Where(s => !s.IsDeleted && s.StartTime >= startDate && s.EndTime <= endDate)
                .OrderBy(s => s.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetShiftsByDateRangeCountAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Shifts
                .Where(s => !s.IsDeleted && s.StartTime >= startDate && s.EndTime <= endDate)
                .CountAsync();
        }

        public async Task<double> GetTotalHoursWorkedAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var shifts = await _context.Shifts
                .Where(s => s.EmployeeId == employeeId && !s.IsDeleted &&
                           s.StartTime >= startDate && s.EndTime <= endDate)
                .ToListAsync();

            return shifts.Sum(s => (s.EndTime - s.StartTime).TotalHours);
        }

        public async Task<Employee> GetEmployeeWithMostHoursAsync(DateTime startDate, DateTime endDate)
        {
            var employeeHours = await _context.Shifts
                .Where(s => !s.IsDeleted && s.StartTime >= startDate && s.EndTime <= endDate)
                .GroupBy(s => s.Employee)
                .Select(g => new
                {
                    Employee = g.Key,
                    TotalHours = g.Sum(s => (s.EndTime - s.StartTime).TotalHours)
                })
                .OrderByDescending(x => x.TotalHours)
                .FirstOrDefaultAsync();

            return employeeHours?.Employee;
        }

        public async Task<double> GetAverageHoursWorkedAsync(DateTime startDate, DateTime endDate)
        {
            var shifts = await _context.Shifts
                .Where(s => !s.IsDeleted && s.StartTime >= startDate && s.EndTime <= endDate)
                .ToListAsync();

            if (!shifts.Any())
                return 0;

            var totalHours = shifts.Sum(s => (s.EndTime - s.StartTime).TotalHours);
            return totalHours / shifts.Count;
        }
    }
}
