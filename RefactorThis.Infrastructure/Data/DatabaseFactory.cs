using NPoco;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    public class DatabaseFactory : INPocoDatabaseFactory
    {
        private const string ConnectionStringName = "DefaultConnectionString";

        public IDatabase CreateDatabase()
        {
            return new Database(ConnectionStringName);
        }
    }
}