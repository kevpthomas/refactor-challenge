using NPoco;
using RefactorThis.Infrastructure.Interfaces;

namespace RefactorThis.Infrastructure.Data
{
    public abstract class Repository
    {
        private readonly INPocoDatabaseFactory _databaseFactory;

        protected abstract string TableName { get; }

        protected Repository(INPocoDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected IDatabase CreateDatabase()
        {
            return _databaseFactory.CreateDatabase();
        }
    }
}