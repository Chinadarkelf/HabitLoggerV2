using Microsoft.Data.Sqlite;

namespace HabitLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // path for Sqlite connection
            string connectionString = @"Data Source=C:\Users\joeyj\Documents\VSProjects\CSharpAcademy\HabitLoggerV2\HabitLoggerV2\habit-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Date TEXT,
                                        Quantity INTEGER
                                        )";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}