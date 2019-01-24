using NPoco;

namespace RefactorThis.Infrastructure.Interfaces
{
    /// <summary>
    /// Defines mechanism to instantiate abstract instances of <see cref="IDatabase"/>.
    /// </summary>
    public interface INPocoDatabaseFactory
    {
        /// <summary>
        /// Create an abstract instance of <see cref="IDatabase"/>.
        /// </summary>
        /// <returns>NPoco database instance.</returns>
        IDatabase CreateDatabase();
    }
}