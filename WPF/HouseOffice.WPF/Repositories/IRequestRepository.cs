using HouseOffice.DAL.Entities;

namespace HouseOffice.WPF.Repositories
{
    public interface IRequestRepository : IRepository<UserRequest>
    {
        Task<List<UserRequest>> GetRequestsByUserIdAsync(int userId);
    }
}
