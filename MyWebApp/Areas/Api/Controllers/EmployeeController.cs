using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Permissions;
using System.Web.Http;
using EmployeeDataAccess;

namespace MyWebApp.Areas.Api.Controllers
{
    public class EmployeeController : ApiController
    {
        public EmployeeController()
        {
        }

        [HttpGet]
        [Route("api/employee/get")]
        public  IHttpActionResult Get()
        {
            IList<Employee> employees = null;
            
            using(var ctx = new EmployeeDBEntities())
            {
                employees = ctx.Employees.ToList();
            }
            
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpGet]
        [Route("api/employee/get/{id}")]
        public Employee Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
            }
        }
        
        [HttpPost]
        [Route("api/employee/postNewEmployee")]
        public IHttpActionResult PostNewEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EmployeeDBEntities())
            {
                ctx.Employees.Add(new Employee()
                {
                    Gender = employee.Gender,
                    FirstName = employee.FirstName,
                    Salary = employee.Salary,
                    ID = employee.ID,
                    LastName = employee.LastName,
                    UpdateTimestamp = DateTime.Now
                });
                
                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    ctx.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                ve.PropertyName,
                                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                                ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }

            return Ok();
        }

        [Route("api/employee/updateEmployee")]
        public IHttpActionResult Put(Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid data");

            using (var ctx = new EmployeeDBEntities())
            {
                var existingEmployee = ctx.Employees.FirstOrDefault(e => e.ID == employee.ID);
                if (existingEmployee != null)
                {
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.Gender = employee.Gender;
                    existingEmployee.Salary = employee.Salary;
                    existingEmployee.UpdateTimestamp = DateTime.Now;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }
        
        [HttpDelete]
        [Route("api/employee/deleteEmployee/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Not a valid Employee Id");

            using (var ctx = new EmployeeDBEntities())
            {
                var employee = ctx.Employees
                    .FirstOrDefault(s => s.ID == id);

                ctx.Entry(employee).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
