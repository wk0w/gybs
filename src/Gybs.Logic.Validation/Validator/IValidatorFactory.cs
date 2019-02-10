namespace Gybs.Logic.Validation.Validator
{
    /// <summary>
    /// Represents a factory for <see cref="IValidator"/>.
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Creates a validator.
        /// </summary>
        /// <returns>The validator.</returns>
        IValidator Create();
    }
}
