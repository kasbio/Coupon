using Kasbio.Coupon.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Kasbio.Coupon.Common.Interfaces
{
    public  interface IAsyncRepository<T> where T: BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> CountAsync(ISpecification<T> spec);

        Task<int> UpdateRangeAsync(T[] ts);
    }
}
