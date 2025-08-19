namespace InterviewTest.Server.Model //namespace are used to organise related classes
{
    /* Define public class.
     * This is data trasfer object (DTO).
     * Groups multiple pieces of information relate to employee.
     * Used to send structured data from backend to frontend in single response.
     */
    public class EmployeeQueryData
    {
        //A property that holds alist of employee objects
        //List<Employee> it can store multiple Employee items
        //{get; set;} - automatically both a property a getter and setter
        //Store all employees retrieved from the database
        public List<Employee> Employees { get; set; }


        //Summary for SumA, SumB and SumC\\
        //A property holds an integer value
        //Used to store some computed total
        public int SumA { get; set; }
        public int SumB { get; set; }
        public int SumC { get; set; }
    }
}
