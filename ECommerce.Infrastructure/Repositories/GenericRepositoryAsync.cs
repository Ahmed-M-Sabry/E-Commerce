using ECommerce.Domain.IRepositories;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;
        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public virtual async Task<T> GetByIdAsync(int id)
        {

            return await _dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();

        }

        public virtual async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable().ToList();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] Includes)
        {
            var data = _dbContext.Set<T>().AsQueryable();
            foreach (var item in Includes)
            {
               data = data.Include(item);
            }
            return await data.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] Includes)
        {
            var data = _dbContext.Set<T>().AsQueryable();
            foreach (var item in Includes)
            {
                data = data.Include(item);
            }
            return await data.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Set<T>().CountAsync();

        }
    }
}
