using HouseOffice.DAL.Entities;

namespace HouseOffice.WPF.Repositories
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<List<Request>> GetRequestsByUserIdAsync(int userId);
    }
}
