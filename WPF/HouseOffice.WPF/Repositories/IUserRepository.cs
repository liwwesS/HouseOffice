using HouseOffice.DAL.Entities;

namespace HouseOffice.WPF.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetUsersWithRoleAsync(string roleType);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserBySNILSAsync(string snils);
    }
}
