
using DSCC._8392.DAL.Context;
using DSCC._8392.Domain.Contracts;
using DSCC._8392.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DSCC._8392.DAL.Repositories
{
    //repository implementation for entities inheriting from BaseEntity
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        public GenericRepository(DbContext context)
        {
            _context = context;
        }
        private DbSet<T> _dbSet => _context.Set<T>();
        public async Task InsertItemAsync(T item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteItemAsync(T item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }
        //if entity is connected to another entity (through FK), it is possible to load related entity 
        //if includes are indicated
        public async Task<T> GetItemByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetWithRelatedTables(includes).SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<T>> GetItemsAsync(params Expression<Func<T, object>>[] includes)
        {
            return await GetDbSetWithRelatedTables(includes).ToListAsync();
        }

        public bool IfExists(int id)
        {
            return _dbSet.Any(t => t.Id == id);
        }
        //method for loading related entities
        private IQueryable<T> GetDbSetWithRelatedTables(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                  (current, include) => current.Include(include));
            }
            //if includes are not indicated, return entities without related entities
            return query;
        }
    }
}
