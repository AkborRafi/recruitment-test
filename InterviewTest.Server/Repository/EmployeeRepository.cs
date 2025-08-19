using InterviewTest.Server.Model;
using Microsoft.Data.Sqlite;

namespace InterviewTest.Server.Repository
{
    public class EmployeeRepository
    {
        private SqliteConnection _conn;

        public EmployeeRepository()
        {
            //SQLite server connection string
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            _conn = new SqliteConnection(builder.ConnectionString);
        }


        /// <summary>
        /// Fetches all employees from the database.
        public List<Employee> GetEmployees()
        {
            // Create a list to store the results from database
            List<Employee> employees = new List<Employee>();
            _conn.Open();

            // Create a command to execute the SQL query
            // new command tied to that specific connection
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Name, Value FROM Employee";

                // Execute the command and read the results, which lets to read the data rows one by one
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            // Get the column index of the "Name" column
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            // Get the column index of the "Value" column
                            Value = reader.GetInt32(reader.GetOrdinal("Value"))
                        });
                        
                    }
                }
            }
            _conn.Close();
            return employees; //return the list of employees objects to the caller
        }
    }
    
}
