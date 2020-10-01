using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using EmployeeDataAccess;
using Newtonsoft.Json;



namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmployeeSalesInfo()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                //HTTP GET
                var responseTask = client.GetAsync("sale/get");
                responseTask.Wait();

                var result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
                
                var readTask = result.Content.ReadAsAsync<IList<Sale>>();
                readTask.Wait();

                var employeeSales = readTask.Result;
                    
                var employeeSalesForChart = employeeSales
                    .GroupBy(s => s.EmployeeName)
                    .Select(sl => new
                    {
                        Employee = sl.FirstOrDefault().EmployeeName,
                        saleAmount = sl.Sum(s => s.Amount)
                    }).ToList();
                    
                return Json(employeeSalesForChart, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult ProductSalesInfo()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                //HTTP GET
                var responseTask = client.GetAsync("sale/get");
                responseTask.Wait();

                var result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
                
                var readTask = result.Content.ReadAsAsync<IList<Sale>>();
                readTask.Wait();

                var sales = readTask.Result;
                    
                var productSalesForChart = sales
                    .GroupBy(s => s.ProductName)
                    .Select(sl => new
                    {
                        Product = sl.FirstOrDefault().ProductName,
                        saleAmount = sl.Sum(s => s.Amount)
                    }).ToList();
                    
                return Json(productSalesForChart, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
