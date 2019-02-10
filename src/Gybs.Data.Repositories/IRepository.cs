using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gybs.Data.Repositories
{
    /// <summary>
    /// Represents a repository of entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Finds an entity with the given primary key values.
        /// </summary>
        /// <param name="keys">The primary keys.</param>
        /// <returns>The entity found, or null.</returns>
        Task<TEntity> FindAsync(params object[] keys);

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>Read-only list of entities.</returns>
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>A task which represents an asynchronous operation.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task which represents an asynchronous operation.</returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <returns>A task which represents an asynchronous operation.</returns>
        Task RemoveAsync(TEntity entity);
    }
}
