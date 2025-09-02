using EmployeeShiftManagementSystem.Core.Entities;


namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync(int pageNumber, int pageSize);
        Task<int> GetActiveEmployeesCountAsync();
        Task<bool> IsEmailUniqueAsync(string email);
        
    }
}
