using HouseOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HouseOffice.DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Role> Roles { get; init; }
        public DbSet<Request> Requests { get; init; }

        public ApplicationContext()
        {
            Task.Run(async () =>
            {
                await Database.EnsureCreatedAsync();
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("SQLServerConnection"));
        }
    }
}
