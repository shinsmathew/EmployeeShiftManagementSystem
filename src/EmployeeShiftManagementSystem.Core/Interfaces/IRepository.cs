using EmployeeShiftManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeShiftManagementSystem.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id); // Changed from GetIdAsync
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // Added proper type
        Task AddAsync(T entity);
        Task UpdateAsync(T entity); // Made async
        Task RemoveAsync(T entity); // Made async
        Task<bool> ExistsAsync(int id);
    }
}
