using System.Configuration;
using System.Data.SqlClient;

namespace RefactorThis.Models
{
    public class Helpers
    {
        public static SqlConnection NewConnection()
        {
            return new SqlConnection(ConfigurationManager.
                ConnectionStrings["DefaultConnectionString"].ConnectionString);
        }
    }
}