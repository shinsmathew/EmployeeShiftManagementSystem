using EmployeeShiftManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync(int pageNumber, int pageSize);
        Task<int> GetActiveEmployeesCountAsync();
        Task<bool> IsEmailUniqueAsync(string email);
        
    }
}
