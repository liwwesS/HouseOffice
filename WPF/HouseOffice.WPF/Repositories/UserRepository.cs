using HouseOffice.DAL;
using HouseOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseOffice.WPF.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsersWithRoleAsync(string roleType)
        {
            return await _context.Users
                .Where(u => u.Roles.RoleType == roleType)
                .ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserBySNILSAsync(string snils)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.SNILS == snils);
        }

        public override async Task AddAsync(User entity)
        {
            await base.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task UpdateAsync(User entity)
        {
            await base.UpdateAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.LastName = user.LastName;
                existingUser.FirstName = user.FirstName;
                existingUser.MiddleName = user.MiddleName;
                existingUser.SNILS = user.SNILS;
                existingUser.Password = user.Password;
                existingUser.PassportSeries = user.PassportSeries;
                existingUser.PassportNumber = user.PassportNumber;
                existingUser.PassportIssued = user.PassportIssued;
                existingUser.PassportDate = user.PassportDate;

                await _context.SaveChangesAsync();
            }
        }

        public override async Task DeleteAsync(User entity)
        {
            await base.DeleteAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
