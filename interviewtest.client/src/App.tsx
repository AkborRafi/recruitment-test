import { useEffect, useState } from 'react';
import { Employee } from './Employee';

function App() {
    const [employeeCount, setEmployeeCount] = useState<number>(0);
    const [employees, setEmployees] = useState<Employee[]>([]);
    useEffect(() => {
        checkConnectivity();
        fetchEmployeeDetails(); //Call the function to fetch employee details here
    }, []);

    return (
        <>
        <div>Connectivity check: {employeeCount > 0 ? `OK (${employeeCount})` : `NOT READY`}</div>
            <div>Complete your app here</div>

            <ul>
                {employees.map((employee, index) => (
                    <li key={index}>
                        Employee: {employee.name}, Value: {employee.value}
                    </li>
                ))
                }
            </ul>
    </>
    );

    async function checkConnectivity() {
        const response = await fetch('api/employees');
        const data = await response.json();
        setEmployeeCount(data.length);
    }

    //Function to fetch employee details from the API
    async function fetchEmployeeDetails() {
        const response = await fetch('api/employees');
        const employeesData: Employee[] = await response.json();
        setEmployees(employeesData);
    }
}

export default App;