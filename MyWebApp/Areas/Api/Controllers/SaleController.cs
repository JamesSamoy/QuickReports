using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApp.Areas.Api.Controllers
{
    public class SaleController : ApiController
    {
        [HttpGet]
        [Route("api/sale/get")]
        public IHttpActionResult Get()
        {
            IList<Sale> sales = null;
            using (var ctx = new EmployeeDBEntities_Sale())
            {
                sales = ctx.Sales.ToList();
            }

            using (var ctxEmp = new EmployeeDBEntities())
            {
                foreach (var sale in sales)
                {
                    sale.EmployeeName = ctxEmp.Employees.FirstOrDefault(e => e.ID == sale.EmployeeId)?.FirstName + " " +
                                        ctxEmp.Employees.FirstOrDefault(e => e.ID == sale.EmployeeId)?.LastName;
                }
            }

            using (var ctxProd = new EmployeeDBEntities_Product())
            {
                foreach (var sale in sales)
                {
                    sale.ProductName = ctxProd.Products.FirstOrDefault(p => p.ProductId == sale.ProductId)?.Name;
                }
            }

            if (sales.Count == 0)
            {
                return NotFound();
            }

            return Ok(sales);
        }

        [HttpGet]
        [Route("api/sale/getEmployeeSalesTotals")]
        public IHttpActionResult GetEmployeeSalesTotals()
        {
            //List<object> employeeSales = new List<object>();
            using (var ctx = new EmployeeDBEntities_Sale())
            {
                var employeeSales = ctx.Sales
                    .GroupBy(s => s.EmployeeId)
                    .Select(sl => new
                    {
                        Employee = sl.FirstOrDefault().EmployeeId,
                        saleAmount = sl.Sum(s => s.Amount)
                    }).ToList();
                
                if (employeeSales.Count == 0)
                {
                    return NotFound();
                }

                return Ok(employeeSales);
            }
        }

        [HttpGet]
        [Route("api/sale/get/{id}")]
        public Sale Get(int id)
        {
            using (var ctx = new EmployeeDBEntities_Sale())
            {
                return ctx.Sales.FirstOrDefault(p => p.SaleId == id);
            }
        }

        [HttpPost]
        [Route("api/sale/postNewSale")]
        public IHttpActionResult PostNewProduct(Sale sale)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EmployeeDBEntities_Sale())
            {
                ctx.Sales.Add(new Sale()
                {
                    ProductId = sale.ProductId,
                    EmployeeId = sale.EmployeeId,
                    CustomerFirstName = sale.CustomerFirstName,
                    CustomerLastName = sale.CustomerLastName,
                    Comment = sale.Comment,
                    Quantity = sale.Quantity,
                    Amount = GetProductPriceByProductId((int)sale.ProductId) * sale.Quantity
                });

                try
                {
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

        [Route("api/sale/updateSale")]
        public IHttpActionResult Put(Sale sale)
        {
            if (!ModelState.IsValid)
                return BadRequest("The data you entered is not valid");

            using (var ctx = new EmployeeDBEntities_Sale())
            {
                var existingSale = ctx.Sales.FirstOrDefault(s => s.SaleId == sale.SaleId);
                if (existingSale != null)
                {
                    existingSale.ProductId = sale.ProductId;
                    existingSale.EmployeeId = sale.EmployeeId;
                    existingSale.CustomerFirstName = sale.CustomerFirstName;
                    existingSale.CustomerLastName = sale.CustomerLastName;
                    existingSale.Comment = sale.Comment;
                    existingSale.Quantity = sale.Quantity;
                    existingSale.Amount = GetProductPriceByProductId((int)sale.ProductId) * sale.Quantity;

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
        [Route("api/sale/deleteSale/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Not a valid Product Id");

            using (var ctx = new EmployeeDBEntities_Sale())
            {
                var sale = ctx.Sales.FirstOrDefault(s => s.SaleId == id);

                ctx.Entry(sale).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

        private decimal GetProductPriceByProductId(int id)
        {
            using(var ctx = new EmployeeDBEntities_Product())
            {
                return ctx.Products.FirstOrDefault(p => p.ProductId == id).Price;
            }
        }

        private string GetEmployeeNameById(int id)
        {
            if (id != null)
            {
                using (var ctx = new EmployeeDBEntities())
                {
                    var first = ctx.Employees.FirstOrDefault(e => e.ID == id)?.FirstName;
                    var last = ctx.Employees.FirstOrDefault(e => e.ID == id)?.LastName;
                    return first + " " + last;
                }
            }

            return null;
        }
    }
}
