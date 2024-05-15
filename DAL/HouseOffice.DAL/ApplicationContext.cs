using HouseOffice.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HouseOffice.DAL
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Request> Requests => Set<Request>();
        public DbSet<UserRequest> UserRequests => Set<UserRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleType = "Admin" },
                new Role { Id = 2, RoleType = "Boss" },
                new Role { Id = 3, RoleType = "Worker" },
                new Role { Id = 4, RoleType = "User" });

            modelBuilder.Entity<Request>().HasData(
                new Request { Id = 1, RequestType = "На улучшение жилищных условий" },
                new Request { Id = 2, RequestType = "Выдача справки о составе семьи" },
                new Request { Id = 3, RequestType = "На обследование дома" },
                new Request { Id = 4, RequestType = "На снятие дома с кадастрого учёта" },
                new Request { Id = 5, RequestType = "Выдача справки о трудовом стаже" });
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
