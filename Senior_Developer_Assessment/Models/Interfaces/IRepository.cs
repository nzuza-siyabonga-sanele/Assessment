using System.Linq.Expressions;

namespace Senior_Developer_Assessment.Models.Interfaces;    

// Generic repository interface for CRUD operations
public interface IRepository<T> where T : class
{
    // Get all entities
    Task<IEnumerable<T>> GetAllAsync();

    // Find entities based on a predicate
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    // Get entity by ID
    Task<T?> GetByIdAsync(int id);

    // Add a new entity
    Task AddAsync(T entity);

    // Update an existing entity
    void Update(T entity);

    // Delete an entity
    void Delete(T entity);

    // Save changes to the data source
    Task<bool> SaveAsync();
}
