using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Models.Enitities;

namespace Senior_Developer_Assessment.Api.Data;

public class DataDbContext(DbContextOptions<DataDbContext> options) : DbContext(options)

    {
    public DbSet<User> Users => Set<User>();
    public DbSet<UserTask> Tasks => Set<UserTask>();
    }

