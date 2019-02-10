using Microsoft.EntityFrameworkCore;

namespace Gybs.Data.Ef
{
    /// <summary>
    /// Extensions for creating <see cref="DbSetQueries{TEntity}"/> - an entry point for the queries as extensions.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Allows to access queries.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="set"><see cref="DbSet{TEntity}"/>.</param>
        /// <returns><see cref="DbSetQueries{TEntity}"/> object for access to the queries.</returns>
        public static DbSetQueries<TEntity> Queries<TEntity>(this DbSet<TEntity> set)
            where TEntity : class
        {
            return new DbSetQueries<TEntity>(set);
        }
    }
}