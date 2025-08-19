using InterviewTest.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

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

                   // It is already done by using statement. (line: 35)
                   // reader.Close();
                }


                /*
                -----------------------------
                *No need to use this code in production!
                *It is only for demonstration purposes.
                *It is already done by using statement. (line:28)
                ------------------------------
                connection.Close();
                connection.Dispose();
                 */
            }

            var sortedEmployees = employees.OrderBy(e => e.Name).ToList();
            return sortedEmployees; //return the sorted list of employees objects to the caller
        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO Employees (Name, Value) VALUES (@name, @value)";
                SqliteParameter nameParam = new SqliteParameter("@name",SqliteType.Text);
                nameParam.Value = employee.Name;
                SqliteParameter valueParam = new SqliteParameter("@value", SqliteType.Integer);
                valueParam.Value = employee.Value;
                cmd.Parameters.AddRange(new[] { nameParam, valueParam });

                try
                {
                    cmd.ExecuteNonQuery();
                    return Ok();
                }
                catch (SqliteException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] Employee employee)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Employees SET Name = @newName, Value = @value WHERE Name = @name";
                cmd.Parameters.AddWithValue("@newName", employee.Name);
                cmd.Parameters.AddWithValue("@value", employee.Value);
                cmd.Parameters.AddWithValue("@name", name);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        return NotFound("Employee not found.");
                    return Ok();
                }
                catch (SqliteException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Employees WHERE Name = @name";
                cmd.Parameters.AddWithValue("@name", name);

                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                        return NotFound("Employee not found.");
                    return Ok();
                }
                catch (SqliteException ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
