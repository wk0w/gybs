using Microsoft.EntityFrameworkCore;

namespace Gybs.Data.Ef;

/// <summary>
/// Extensions for creating <see cref="DbContextQueries" /> - an entry point for the queries as extensions.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Allows to access queries.
    /// </summary>
    /// <param name="context"><see cref="DbContext"/>.</param>
    /// <returns><see cref="DbContextQueries"/> object for access to the queries.</returns>
    public static DbContextQueries Queries(this DbContext context)
    {
        return new DbContextQueries(context);
    }
}
