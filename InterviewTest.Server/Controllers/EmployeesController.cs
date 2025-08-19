using InterviewTest.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace InterviewTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public List<Employee> Get()
        {

            //ADO.NET have individual types such as SqlConnection, SqliteConnection, OracleConnection etc.
            //SqliteConnection is used to connect to Sqlite database.
            //SqlConnection is used to connect to Sql Server database.
            //SqlConnection
            //SqlCommand
            //SqlAdapter
            //SqlDataReader
            //SqlDataAdapter


            var employees = new List<Employee>();

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees";
                using (var reader = queryCmd.ExecuteReader())
                {

                    //**Note: GetOrdinal is used to get the value of a column by its name.**// 
                    
                    //Get the collumn index of the "Name" column
                    var nameOrdinal = reader.GetOrdinal("Name");

                    //Get the collumn index of the "Value" column
                    var valueOrdinal = reader.GetOrdinal("Value");

                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            //C# are equivalent of NVARCHAR(Name) where Name is declare by string
                            //**Name = reader.GetString(0),
                            Name = reader.GetString(nameOrdinal),

                            //C# are equivalent of INT(Value) where Value is declare by integer (Int32)
                            //Value = reader.GetInt32(1)
                            Value = reader.GetInt32(valueOrdinal)
                        });
                    }
                }


                /*
                -----------------------------
                *No need to use this code in production!
                *It is only for demonstration purposes.
                *It is already using by using statement. (line:28)
                ------------------------------
                connection.Close();
                connection.Dispose();
                 */
            }

            return employees;
        }
    }
}
