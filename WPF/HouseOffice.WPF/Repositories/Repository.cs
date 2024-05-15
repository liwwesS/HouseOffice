using HouseOffice.DAL;
using Microsoft.EntityFrameworkCore;

namespace HouseOffice.WPF.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public Repository(ApplicationContext context)
        {
            _context = context;
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async virtual Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async virtual Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
