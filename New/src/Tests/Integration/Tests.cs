using System.Data;
using System.Data.SqlClient;

namespace Tests.Integration
{
    public class Tests
    {
        internal const string ConnectionString = @"Server=VLADIMIR-PC\SQL2014;Database=AutoBuyer;Trusted_Connection=true;";

        public Tests()
        {
            ClearDatabase();
        }

        private void ClearDatabase()
        {
            string query = "DELETE FROM dbo.Buyer";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.Text
                };

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
