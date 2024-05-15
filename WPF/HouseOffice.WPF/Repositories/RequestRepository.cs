using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseOffice.WPF.Repositories
{
    public class RequestRepository : Repository<UserRequest>, IRequestRepository
    {
        private readonly ApplicationContext _context;

        public RequestRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<UserRequest>> GetRequestsByUserIdAsync(int userId)
        {
            return await _context.UserRequests
                .Include(r => r.Requests)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public override async Task AddAsync(UserRequest entity)
        {
            await base.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(UserRequest entity)
        {
            await base.UpdateAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(UserRequest entity)
        {
            await base.DeleteAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
