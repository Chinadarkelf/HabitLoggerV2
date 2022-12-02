using Microsoft.Data.Sqlite;

namespace HabitLogger
{
    internal class Program
    {
        // path for Sqlite connection
        static string connectionString = @"Data Source=C:\Users\joeyj\Documents\VSProjects\CSharpAcademy\HabitLoggerV2\HabitLoggerV2\habit-Tracker.db";
        static void Main(string[] args)
        {
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

            GetUserInput();
        }

        static void GetUserInput()
        {
            bool close = false;

            ShowMenu();

            while (!close)
            {
                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        close = true;
                        break;
                    case "1":
                        Console.Clear();
                        GetAllRecords();
                        Console.WriteLine("Enter any key to go back to menu");
                        Console.ReadLine();
                        Console.Clear();
                        ShowMenu();
                        break;
                    case "2":
                        Console.Clear();
                        InsertRecord();
                        break;
                    case "3":
                        Console.Clear();
                        UpdateRecord();
                        break;
                    case "4":
                        Console.Clear();
                        DeleteRecord();
                        break;
                    default:
                        Console.WriteLine("Invalid command, please try again.");
                        break;
                }
            }
        }

        private static void InsertRecord()
        {
            // Grab the time when InsertRecord is initiated
            string time = DateTime.Now.ToString();

            // Prompt user for amount of water consumed
            string quantity = GetNumberInput();

            // To modify/create tables, a new connection must be made/closed, with a tableCmd being created/executed
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                // SQL command to be executed stored in the tableCmd variable
                tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{time}', {quantity})";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"Successfully entered {quantity} at {time}");

            ReturnToMenu();
        }

        private static void DeleteRecord()
        {
            Console.Clear();
            if (GetAllRecords() == 1) {
                Console.WriteLine("Enter id of table entry to delete:");
                string response = Console.ReadLine();
                if (response.Equals("")) {
                    Console.WriteLine("Invalid response, please try again.");
                    response = Console.ReadLine();
                }

                try
                {
                    int idToDelete = int.Parse(response);

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        var tableCmd = connection.CreateCommand();

                        tableCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = '{idToDelete}'";

                        int rowsChanged = tableCmd.ExecuteNonQuery();
                        if (rowsChanged == 0)
                        {
                            Console.WriteLine($"Record with ID {idToDelete} does not exist");
                            ReturnToMenu();
                        }
                        else
                        {
                            Console.WriteLine($"Record with ID {idToDelete} was succesfully deleted.");
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                
            }

            ReturnToMenu();
        }

        private static void UpdateRecord()
        {
            if (GetAllRecords() == 1)
            {
                Console.WriteLine("Enter the id of the record you want to update:");
                var id = int.Parse(Console.ReadLine());
                Console.WriteLine($"Enter the amount you would like to change ID {id} to");
                var input = Console.ReadLine();

                try
                {
                    double newAmount = double.Parse(input);

                    using (var connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        var tableCmd = connection.CreateCommand();

                        tableCmd.CommandText = $"UPDATE drinking_water " +
                                               $"SET Quantity = {newAmount} " +
                                               $"WHERE Id = {id}";

                        tableCmd.ExecuteNonQuery();
                        connection.Close();
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            ReturnToMenu();
            
        }

        public static int GetAllRecords()
        {
            // As with all table manipulation, a new connection must be created
            using (var connection = new SqliteConnection(connectionString))
            {
                // Boilerplate code for creating a new connection {Open, and initialize a tableCmd}
                connection.Open();
                var tableCmd = connection.CreateCommand();

                // List of custom object to store data from table to present to user
                List<DrinkingWater> tableData = new();

                tableCmd.CommandText = $"SELECT * FROM drinking_water";

                // Create a reader object which iterates through a table
                SqliteDataReader reader = tableCmd.ExecuteReader(); 

                // Standard check for data
                if (reader.HasRows)
                {
                    // Loop to iterate through all data of the table
                    while (reader.Read())
                    {
                        // For each row in the table, create a new object to add to the List<T>
                        tableData.Add(new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Quantity = reader.GetDouble(2)
                        });
                    }
                } else
                {
                    Console.WriteLine("No rows exist in table");
                    return 0;
                }
                connection.Close();

                Console.WriteLine("-------------------------------------\n");

                foreach (var row in tableData)
                {
                    Console.WriteLine($"{row.Id} - {row.Date} - Quantity: {row.Quantity}");
                }

                Console.WriteLine("-------------------------------------\n");
                return 1;
            }
        }

        private static string GetNumberInput()
        {
            Console.WriteLine("Please enter amount of water consumed, in measurement of your choice.");

            string userInput = Console.ReadLine();
            return userInput;
        }

        // Seperate ShowMenu() method to allow seamless transition between functions -- leaving InsertRecord() to go back to menu
        // When called GetUserInput() within InsertRecord(), showed nested menus and required 'exiting' twice
        private static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine(@"--------------MAIN MENU--------------
0 - Close the application
1 - View all records
2 - Insert record
3 - Update record
4 - Delete record
-------------------------------------");
        }

        private static void ReturnToMenu()
        {
            Console.WriteLine("Press any key to return to menu");
            Console.ReadLine();
            ShowMenu();
        }
    }
}

public class DrinkingWater
{
    public int Id { get; set; }
    public string Date { get; set; }
    public double Quantity { get; set; }
}