using System.Threading.Tasks;

namespace Gybs.Data.Repositories;

/// <summary>
/// Represents a unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made within unit to the underlying persistence mechanism.
    /// </summary>
    /// <returns>A task which represents an asynchronous operation.</returns>
    Task SaveChangesAsync();
}
