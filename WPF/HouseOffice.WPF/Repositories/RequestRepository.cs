using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseOffice.WPF.Repositories
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly ApplicationContext _context;

        public RequestRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Request>> GetRequestsByUserIdAsync(int userId)
        {
            return await _context.Requests
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public override async Task AddAsync(Request entity)
        {
            await base.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(Request entity)
        {
            await base.UpdateAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(Request entity)
        {
            await base.DeleteAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
