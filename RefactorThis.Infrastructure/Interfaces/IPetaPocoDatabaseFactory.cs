using NPoco;

namespace RefactorThis.Infrastructure.Interfaces
{
    public interface INPocoDatabaseFactory
    {
        IDatabase CreateDatabase();
    }
}