using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameResultsRepository
{
    public class GameResultRepository : IDisposable
    {
        private readonly string connectionString;
        private SqlConnection connection;

        public GameResultRepository(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new SqlConnection(connectionString);
        }

        public async Task InsertOption1Result(string winnerName, string victimName, int iteration, int powerLevel)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                string query = "INSERT INTO GameResults (WinnerName, VictimName, Iteration, PowerLevel) VALUES (@WinnerName, @VictimName, @Iteration, @PowerLevel)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WinnerName", winnerName);
                    command.Parameters.AddWithValue("@VictimName", victimName);
                    command.Parameters.AddWithValue("@Iteration", iteration);
                    command.Parameters.AddWithValue("@PowerLevel", powerLevel);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting option 1 result: {ex.Message}");
            }
        }

        public async Task InsertOption2Result(int iteration)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                string query = "INSERT INTO GameResults (Iteration) VALUES (@Iteration)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Iteration", iteration);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting option 2 result: {ex.Message}");
            }
        }


        public void Dispose()
        {
            connection.Dispose();
        }
    }
}
