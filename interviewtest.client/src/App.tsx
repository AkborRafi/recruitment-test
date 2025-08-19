import { useEffect, useState } from 'react';
import { Employee } from './Employee';
import { EmployeeQueryData } from "./EmployeeQueryData";

//importing css styles from App.css
import './App.css';

function App() {
    const [employeeCount, setEmployeeCount] = useState<number>(0);
    const [employees, setEmployees] = useState<Employee[]>([]);
    const [ employeeQueryData, setEmployeeQueryData] = useState<EmployeeQueryData>();
    const [loading, setLoading] = useState<boolean>(false);
    const [showSum, setShowSum] = useState<boolean>(false);

    // For add/edit form
    const [formName, setFormName] = useState('');
    const [formValue, setFormValue] = useState<number>(0);
    const [editIndex, setEditIndex] = useState<number | null>(null);

    useEffect(() => {
        checkConnectivity();
        fetchEmployeeDetails();
    }, []);

    async function checkConnectivity() {
        const response = await fetch('api/employees', { method : 'GET'});
        const data = await response.json();
        setEmployeeCount(data.length);
    }


    // Fetch employee details from the API
    async function fetchEmployeeDetails() {
        const response = await fetch('api/employees');
        const employeesData: Employee[] = await response.json();
        setEmployees(employeesData);
    }

    // Fetch employee queries with Name starting E

    async function fetchEmployeeQueries() {
        setLoading(true);
        const [response] = await Promise.all([
            fetch("api/list/FetchEmployeeQueries"),
            new Promise(resolve => setTimeout(resolve, 5000))
        ]);
        if (!response.ok) {
            const text = await response.text();
            setLoading(false);
            throw new Error(`API error: ${text}`);
        }
        const employeesData: EmployeeQueryData = await response.json();
        setEmployeeQueryData(employeesData);
        setLoading(false);
        setShowSum(true);
    }

    
    /*
    -------------------------------------------------------------
    *** CRUD (Create, Read, Update and Delete Functionalities ***
    -------------------------------------------------------------
    */


    // Add and update employee
    async function handleFormSubmit(e: React.FormEvent) {
        e.preventDefault();
        let response: Response;
        const isAddition = editIndex === null;

        if (isAddition) {
            // Add
            response = await fetch('api/employees', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: formName, value: formValue })
            });
        } else {
            // Update
            response = await fetch(`api/employees/${employees[editIndex].name}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: formName, value: formValue })
            });
        }

        if (response.ok) {
            window.alert(isAddition ? 'Add successful!' : 'Update successful!');
            fetchEmployeeDetails();
            checkConnectivity();
            setFormName('');
            setFormValue(0);
            setEditIndex(null);
        } else {
            const errorText = await response.text();
            window.alert(`Error: ${errorText}`);
        }      
    }

    // Delete employee 
    async function handleDelete(index: number) {
        const response = await fetch(`api/employees/${employees[index].name}`, { method: 'DELETE' });
        if (response.ok) {
            window.alert('Employee deleted successfully!');
            fetchEmployeeDetails();
            checkConnectivity();
        } else {
            const errorText = await response.text();
            window.alert(`Error deleting employee: ${errorText}`);
        }
    }

    // Start editing
    function handleEdit(index: number) {
        setFormName(employees[index].name);
        setFormValue(employees[index].value);
        setEditIndex(index);
    }

    return (
        <>
            <div>Connectivity check: {employeeCount > 0 ? `OK (${employeeCount})` : `NOT READY`}</div>
            <div>Complete your app here</div>

            <h2>Original Employee List</h2>
            <div style={{ height: "500px", overflowY: "auto", border: "2px solid black", borderRadius: "10px", padding: "10px" }}>
                <ul className="employeeList">
                    {employees.map((employee, index) => (
                        <li key={index} className="listItem">
                            Employee: {employee.name}, Value: {employee.value}
                            <button className="customBtn" onClick={() => handleEdit(index)} style={{ marginLeft: 8 }}>Edit</button>
                            <button className="customBtn" onClick={() => handleDelete(index)} style={{ marginLeft: 4 }}>Delete</button>
                        </li>
                    ))}
                </ul>
            </div>


            {/* Add and Edit Employee Form*/}
            <h3>{editIndex === null ? "Add Employee" : "Edit Employee"}</h3>
            <form onSubmit={handleFormSubmit} style={{ marginBottom: 16 }}>
                <input
                    type="text"
                    placeholder="Name"
                    value={formName}
                    onChange={e => setFormName(e.target.value)}
                    required
                />
                <input
                    type="number"
                    placeholder="Value"
                    value={formValue}
                    onChange={e => setFormValue(Number(e.target.value))}
                    required
                    style={{ marginLeft: "5px" }}
                />
                <button style={{ marginLeft: "5px" }} className="customBtn" type="submit">{editIndex === null ? "Add" : "Update"}</button>
                {editIndex !== null && <button type="button" className="customBtn" style={{ marginLeft: "5px" }} onClick={() => { setEditIndex(null); setFormName(''); setFormValue(0); }}>Cancel</button>}
            </form>

            <h2>Employee Query Data</h2>
            <button className="customBtn" style={{ marginLeft: "5px" }} onClick={fetchEmployeeQueries}>Fetch Employees Queries</button>
            <div style={{ height: "500px", overflowY: "auto", border: "2px solid black", borderRadius: "10px", padding: "10px", marginTop: "10px" }}>
                {loading ? (
                    <div style={{ textAlign: "center", padding: "2em" }}>Loading...</div>
                ) : (
                    <>
                        <ul>
                            {employeeQueryData?.employees.map((employee, index) => (
                                <li key={index}>
                                    Employee: {employee.name}, Value: {employee.value}
                                </li>
                            ))}
                        </ul>
                    </>
                )}
            </div>
            
            <div style={{ display: showSum ? "block" : "none", marginTop: "10px" }}>
                <p style={{ fontSize: "1.5rem" }}>Sum of Values of Employees starting with A: <b>{employeeQueryData?.sumA >= 11171 ? `${employeeQueryData?.sumA}` : `Not greater than or equal to 11171`}</b></p>
                <p style={{ fontSize: "1.5rem" }}>Sum of Values of Employees starting with B: <b>{employeeQueryData?.sumB >= 11171 ? `${employeeQueryData?.sumB}` : `Not greater than or equal to 11171`}</b></p>
                <p style={{ fontSize: "1.5rem" }}>Sum of Values of Employees starting with C: <b>{employeeQueryData?.sumC >= 11171 ? `${employeeQueryData?.sumC}` : `Not greater than or equal to 11171`}</b></p>
            </div>
        </>
    );
}


export default App;