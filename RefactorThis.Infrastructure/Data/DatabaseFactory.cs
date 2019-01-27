using NPoco;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    /// <summary>
    /// Provides mechanism to instantiate an NPoco database.
    /// </summary>
    public class DatabaseFactory : INPocoDatabaseFactory
    {
        // Connection string is defined in web.config
        private const string ConnectionStringName = "DefaultConnectionString";

        public IDatabase CreateDatabase()
        {
            return new Database(ConnectionStringName);
        }
    }
}