using Microsoft.EntityFrameworkCore;

namespace Gybs.Data.Ef
{
    /// <summary>
    /// Represents an entry point for query extensions on <see cref="DbContext"/>.
    /// </summary>
    public sealed class DbContextQueries
    {
        /// <summary>
        /// Gets the <see cref="DbContext"/>.
        /// </summary>
        public DbContext Context { get; }

        internal DbContextQueries(DbContext context)
        {
            Context = context;
        }
    }
}
