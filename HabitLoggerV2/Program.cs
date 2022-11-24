using Microsoft.Data.Sqlite;

namespace HabitLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // path for Sqlite connection
            var connectionString = @"Data Source=habit-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"CREATE TABLE NOT EXISTS drinking_water
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT
                                        Date TEXT
                                        Quantity INTEGER";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}