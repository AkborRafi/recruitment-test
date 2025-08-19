using Microsoft.AspNetCore.Mvc;
using InterviewTest.Server.Model;
using InterviewTest.Server.Repository;

namespace InterviewTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        // This controller is intended to handle list-related API methods.
        //Only allowed to read data from the database.

        private readonly ILogger<ListController> _logger;
        private readonly EmployeeRepository _repo;

        //Dependency Injection
        public ListController(EmployeeRepository repo)
        {
            _repo = repo;
        }

        /*
         * List API methods goes here
         */

        //this will match to route 'app/list'

        [HttpGet("FetchEmployeeQueries")]
        public async Task<EmployeeQueryData> FetchEmployeeQueries()
        {
            /////***** Summary for the API method *****\\\\\

            //Increment Value of the list item\\

            //1- If name starts with 'E', increment by +1
            //2- If name starts with 'G', increment by +10
            //3- All other names increment by +100
            //List the sum of all values for all names starting with 'A', 'B', 'C' whit condition  only present the data where the summed values are greater than or equals to 11,171
            EmployeeQueryData queryData = new EmployeeQueryData();
            var orginalEmployees = _repo.GetEmployees();

            List<Employee> tempEmployees = new List<Employee>();
            foreach (Employee em in orginalEmployees)
            {
                var tempEmployee = em;

                //starrting with 'E'
                if (em.Name.ToLower().StartsWith("e"))
                {
                    tempEmployee.Value += 1;
                }

                //Starting with 'G'
                else if (em.Name.ToLower().StartsWith("g"))
                {
                    tempEmployee.Value += 10;
                }

                //All other names
                else
                {
                    tempEmployee.Value += 100;
                }

                tempEmployees.Add(tempEmployee);
            }

            queryData.Employees = tempEmployees;

            //LINQ queries and I am chaining the "Where" and "Sum" methods together to filter and sum the values in a single query.
            var sumA = orginalEmployees.Where(em => em.Name.ToLower().StartsWith("a")).Sum(em => em.Value);
            var sumB = orginalEmployees.Where(em => em.Name.ToLower().StartsWith("b")).Sum(em => em.Value);
            var sumC = orginalEmployees.Where(em => em.Name.ToLower().StartsWith("c")).Sum(em => em.Value);

            queryData.SumA = sumA;
            queryData.SumB = sumB;
            queryData.SumC = sumC;

            return queryData;
        }

        
    }
        /*************** Under Review ****************/ //Results not working or use
        /*************** Logic is not in correct path way ***************/

        /*[HttpPost("increment")]
        public async Task<IActionResult> IncrementValues()
        {
            var employees = await_context.Employees.ToListAsync();

            foreach(var e in employees)
            {   if (e.Name.StartsWith("E", StringComparison.OrdinalIgnoreCase))
                {
                    e.Value += 1;
                }
                else if (e.Name.StartsWith("G", StringComparison.OrdinalIgnoreCase))
                {
                    e.Value += 10;
                }
                else
                {
                    e.Value += 100;
                }
            }

            await _context.SaveChangesAsync();

            //return updated list so frontend get refreshes

            return Ok(emplyees);
        }*/

        /*
         <summary>
         *Get the sum of all values in the list which starts with 'A','B','C'
         *Only return if the sum is greater than or equal; >=11171 
         *Otherwise return NotFound
         </summary>
         */
        /*
        [HttpGet("sum-abc")]
        public async Task<IActionResult> GetSumABC()
        {
            var sum = await_context.Employees
                .Where(e => e.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase) ||
                            e.Name.StartsWith("B", StringComparison.OrdinalIgnoreCase) ||
                            e.Name.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                .SumAsync(e => e.Value);

            if (sum >= 11171)
                return Ok (new { Sum = sum });

            // If sum is less than 11171, message will show return NotFound
            return Ok(new {Message = "No result >= 11171" });
        }*/


        /*
        public async Task<IActionResult> IncrementValue(string name)
        {
            // Logic to increment the value based on the name prefix
            int incrementValue = 100; // Default increment value
            
            if (name.StartsWith("E"))
            {
                incrementValue = 1;
            }
            else if (name.StartsWith("G"))
            {
                incrementValue = 10;
            }
            // Here you would typically update the database or data source with the new value
            // For demonstration, we will just return the incremented value
            
            return Ok(new { Name = name, IncrementedValue = incrementValue });
        }
        */

        /*
        public async Task<IActionResult> GetSumABC()
        {
            var employees = await _context.Employees.ToListAsync();
            var sum = employees
                .Where(e => e.Name.StartsWith("A", StringComparison.OrdinalIgnoreCase) ||
                            e.Name.StartsWith("B", StringComparison.OrdinalIgnoreCase) ||
                            e.Name.StartsWith("C", StringComparison.OrdinalIgnoreCase))
                .Sum(e => e.Value);
            if (sum >= 11171)
            {
                return Ok(new { Sum = sum });
            }
            return NotFound("Sum is less than 11171");
        }
        */
}