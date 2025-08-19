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
                cmd.CommandText = "SELECT Name, Value FROM Employees";

                // Execute the command and read the results, which lets to read the data rows one by one
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        });
                    }
                }
            }
            _conn.Close();
            var sortedEmployees = employees.OrderBy(e => e.Name).ToList();
            return sortedEmployees; //return the sorted list of employees objects to the caller
        }

        public List<Employee> GetEmployees(bool adultEmployees)
        {
            return new List<Employee>();
        }
    }
    
}
