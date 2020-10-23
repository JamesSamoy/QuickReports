using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using EmployeeDataAccess;



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
            var sales = GetSales();
                
            var employeeSalesForChart = sales
                .GroupBy(s => s.EmployeeName)
                .Select(sl => new
                {
                    Employee = sl.FirstOrDefault().EmployeeName,
                    saleAmount = sl.Sum(s => s.Amount)
                }).ToList();
                
            return Json(employeeSalesForChart, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult ProductSalesInfo()
        {
            var sales = GetSales();
                    
            var productSalesForChart = sales
                .GroupBy(s => s.ProductName)
                .Select(sl => new
                {
                    Product = sl.FirstOrDefault().ProductName,
                    saleAmount = sl.Sum(s => s.Amount)
                }).ToList();
                
            return Json(productSalesForChart, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public FileResult ExportProductSalesTotals()
        {
            var sales = GetSales();
            var prodSalesTotals = sales
                .GroupBy(s => s.ProductName)
                .Select(sl => new
                {
                    Product = sl.FirstOrDefault().ProductName,
                    saleAmount = sl.Sum(s => s.Amount)
                }).ToList<object>();

            //Insert the Column Names.
            prodSalesTotals.Insert(0, "Product" + ',' + "Totals");

            return CreateCsvFile(prodSalesTotals, "ProductSalesTotals.csv");
        }

        [HttpPost]
        public FileResult ExportEmployeeSalesTotals()
        {
            var sales = GetSales();
            var employeeSalesTotals = sales
                .GroupBy(s => s.EmployeeName)
                .Select(sl => new
                {
                    Employee = sl.FirstOrDefault().EmployeeName,
                    saleAmount = sl.Sum(s => s.Amount)
                }).ToList<object>();

            //Insert the Column Names.
            employeeSalesTotals.Insert(0, "Employee" + ',' + "Totals");

            return CreateCsvFile(employeeSalesTotals, "EmployeeSalesTotals.csv");
        }

        private FileResult CreateCsvFile(IList<object> data, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                string[] sale = new[] {data[i].ToString()};
                for (int j = 0; j < sale.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(sale[j] + ',');
                }
 
                //Append new line character.
                sb.Append("\r\n");
 
            }
 
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", fileName);
        }

        public IList<Sale> GetSales()
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

                return readTask.Result;
            }
        }
    }
}
