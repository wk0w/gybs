using Gybs.Logic.Validation.Internal;
using System.Threading.Tasks;

namespace Gybs.Logic.Validation;

/// <summary>
/// Represents a validation rule for the data.
/// </summary>
/// <typeparam name="TValidationData">The type of data to validate.</typeparam>
public interface IValueValidationRule<in TValidationData> : IValidationRule
{
    /// <summary>
    /// Validates the data.
    /// </summary>
    /// <param name="data">The data to validate.</param>
    /// <returns>The result of the validation.</returns>
    ValueTask<IResult> ValidateAsync(TValidationData data);
}
