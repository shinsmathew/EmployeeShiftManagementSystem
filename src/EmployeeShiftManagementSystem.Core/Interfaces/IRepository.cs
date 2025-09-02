using EmployeeShiftManagementSystem.Core.Entities;


namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id); 
        Task AddAsync(T entity);
        Task UpdateAsync(T entity); 
    }
}
