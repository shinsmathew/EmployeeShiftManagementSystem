using EmployeeShiftManagementSystem.Core.Entities;
using EmployeeShiftManagementSystem.Core.Interfaces;
using EmployeeShiftManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


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
            return await _context.Employees.AsNoTracking()
                .Include(e => e.Shifts)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            _context.Employees.Update(entity);
            await _context.SaveChangesAsync(); 
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

