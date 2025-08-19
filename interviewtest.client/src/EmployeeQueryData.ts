// EmployeeQueryData.ts
// Data structure for querying employee information

import { Employee } from './Employee';
export interface EmployeeQueryData {

    employees: Employee[]; // Array of employees matching the query
    sumA: number; // Sum of a specific property (e.g., salary)
    sumB: number; // Sum of another specific property (e.g., bonus) 
    sumC: number; // Sum of a third specific property (e.g., commission)

    /*id?: number;
  name?: string;
  department?: string;
  title?: string;
  isActive?: boolean;
  hireDateFrom?: Date;
  hireDateTo?: Date;*/
}