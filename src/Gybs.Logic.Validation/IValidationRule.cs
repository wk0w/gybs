using System.Threading.Tasks;
using Gybs.Logic.Validation.Internal;

namespace Gybs.Logic.Validation
{
    /// <summary>
    /// Represents a data validation rule.
    /// </summary>
    /// <typeparam name="TValidationData">The type of data to validate.</typeparam>
    public interface IValidationRule<in TValidationData> : IValidationRule
    {
        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <param name="data">The data to validate.</param>
        /// <returns>The result of validation.</returns>
        Task<IResult> ValidateAsync(TValidationData data);
    }
}
