using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Data.Data;
using RealEstateAdmin.Core.Domain.RepositoryContracts;
using System.Linq.Expressions;

namespace RealEstateAdmin.Core.Domain.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RealEstateDbContext _context;
        public GenericRepository(RealEstateDbContext context)
        {
            _context = context;
        }
        public async Task<T> Add(T entity)
        {
            return (await _context.Set<T>().AddAsync(entity)).Entity;

        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }
        public async Task<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(expression);
            return result ?? throw new InvalidOperationException("No entity was found matching the specified condition.");
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            return result ?? default!;
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public T Update(T entity)
        {
            return _context.Set<T>().Update(entity).Entity;
        }
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }
    }
}
