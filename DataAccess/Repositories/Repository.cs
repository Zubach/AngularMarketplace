using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            this._context = context;
            this._dbSet = this._context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await this._dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            this._dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await this._dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            this._dbSet.Update(entity);
        }

        
    }
}
