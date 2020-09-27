using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MyWebApp.Controllers
{
    public class SaleController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Sale> sales = null;

            //
            // TODO: Create HttpSeriveWrapper to take care of Post/Get/Delete calls
            //
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");
                var responseTask = client.GetAsync("sale/get");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Sale>>();
                    readTask.Wait();

                    sales = readTask.Result;
                }
                else
                {
                    //implement logging here
                    sales = Enumerable.Empty<Sale>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator");
                }
            }

            return View(sales);
        }

        public ActionResult Create()
        {
            Sale saleModel = new Sale();

            using (var prodCtx = new EmployeeDBEntities_Product())
            {
                saleModel.ProductCollection = prodCtx.Products.ToList<Product>();
            }

            using(var empCtx = new EmployeeDBEntities())
            {
                saleModel.EmployeeCollection = empCtx.Employees.ToList<Employee>();
            }

            return View(saleModel);
        }

        [HttpPost]
        public ActionResult Create(Sale sale)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<Sale>("sale/postNewSale", sale);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(sale);
        }

        public ActionResult Edit(int id)
        {
            Sale sale = new Sale();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");

                var responseTask = client.GetAsync("sale/get/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Sale>();
                    readTask.Wait();

                    sale = readTask.Result;
                }
            }

            using (var prodCtx = new EmployeeDBEntities_Product())
            {
                sale.ProductCollection = prodCtx.Products.ToList<Product>();
            }

            using (var empCtx = new EmployeeDBEntities())
            {
                sale.EmployeeCollection = empCtx.Employees.ToList<Employee>();
            }

            return View(sale);
        }

        [HttpPost]
        public ActionResult Edit(Sale sale)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");

                var putTask = client.PutAsJsonAsync("sale/updateSale", sale);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(sale);
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44350/api/");

                var deleteTask = client.DeleteAsync("sale/deleteSale/" + id);
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