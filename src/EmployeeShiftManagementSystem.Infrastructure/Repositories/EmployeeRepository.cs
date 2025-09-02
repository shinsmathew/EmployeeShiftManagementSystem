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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Shifts)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> FindAsync(System.Linq.Expressions.Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Employees
                .Include(e => e.Shifts)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task AddAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(Employee entity)
        {
            _context.Employees.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(Employee entity)
        {
            _context.Employees.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync(int pageNumber, int pageSize)
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetActiveEmployeesCountAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .CountAsync();
        }

       
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Employees.AnyAsync(e => e.Email == email);
        }
    }
}

