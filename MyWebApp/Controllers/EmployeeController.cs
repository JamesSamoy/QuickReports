using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using EmployeeDataAccess;

namespace MyWebApp.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Employee> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                //HTTP GET
                var responseTask = client.GetAsync("employee/get");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Employee>>();
                    readTask.Wait();

                    employees = readTask.Result;
                }
                else //web api sent error response
                {
                    //log response status here...
                    employees = Enumerable.Empty<Employee>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
            }
            return View(employees);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                
                //HTTP POST
                var postTask = client.PostAsJsonAsync<Employee>("employee/postNewEmployee", employee);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(employee);
        }

        public ActionResult Edit(int id)
        {
            Employee employee = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                
                //http get
                var responseTask = client.GetAsync("employee/updateEmployee?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Employee>();
                    readTask.Wait();

                    employee = readTask.Result;
                }
            }

            return View(employee);
        }
        
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                
                //http get
                var putTask = client.PutAsJsonAsync("employee/updateEmployee", employee);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(employee);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                
                //HTTP DELETE
                var deleteTask = client.DeleteAsync("employee/deleteEmployee/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}