using Microsoft.EntityFrameworkCore;

namespace Gybs.Data.Ef
{
    /// <summary>
    /// Represents an entry point for query extensions on <see cref="DbSet{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    public sealed class DbSetQueries<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets the <see cref="DbSet{TEntity}"/> with entities.
        /// </summary>
        public DbSet<TEntity> Entities { get; }

        internal DbSetQueries(DbSet<TEntity> entities)
        {
            Entities = entities;
        }
    }
}
