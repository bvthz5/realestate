using System.Linq.Expressions;

namespace RealEstateUser.Core.Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
    }
}
