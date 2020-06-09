using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employeeList;

        public EmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() {Id = 1 , Name = "Mahmoud" , Email = "Mahmoud@gmail.com" , Department = Dept.Development},
                new Employee() {Id = 2 , Name = "Ahmed" , Email = "Ahmed@gmail.com" , Department = Dept.Design},
                new Employee() {Id = 3 , Name = "Ali" , Email = "Ali@gmail.com" , Department = Dept.Security}
            };
        }
        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault( e => e.Id == Id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.Id =_employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeUpdates)
        {
            Employee employee = GetEmployee(employeeUpdates.Id);

            if (employee != null)
            {
                employee.Name = employeeUpdates.Name;
                employee.Email = employeeUpdates.Email;
                employee.Department = employeeUpdates.Department;
            }

            return employee;
        }

        public Employee Delete(int Id)
        {
          Employee employee =  GetEmployee(Id);

          if (employee != null)
          {
              _employeeList.Remove(employee);
          }

          return employee;
        }
    }
}
