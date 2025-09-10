using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Models.Entities;

namespace Senior_Developer_Assessment.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserTask> Tasks => Set<UserTask>();
    }
}
