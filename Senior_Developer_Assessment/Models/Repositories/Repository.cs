using Microsoft.EntityFrameworkCore;
using Senior_Developer_Assessment.Data;
using Senior_Developer_Assessment.Models.Interfaces;
using System.Linq.Expressions;

namespace Senior_Developer_Assessment.Models.Repositories;

/// Generic repository implementation for CRUD operations
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DataDbContext _context;
    protected readonly DbSet<T> _dbSet;


    public Repository(DataDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // Get entity by ID
    public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    // Get all entities
    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    // Find entities based on a predicate
    public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate) =>
    await _dbSet.FirstOrDefaultAsync(predicate);

    // Add a new entity
    public virtual async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

    // Update an existing entity
    public virtual void Update(T entity) => _dbSet.Update(entity);

    // Delete an entity
    public virtual void Delete(T entity) => _dbSet.Remove(entity);

    // Save changes to the database
    public virtual async Task<bool> SaveAsync() => await _context.SaveChangesAsync() > 0;
}