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
    public class ProductController : ApiController
    {
        public ProductController()
        {
        }

        [HttpGet]
        [Route("api/product/get")]
        public IHttpActionResult Get()
        {
            IList<Product> products = null;
            using(var ctx = new EmployeeDBEntities_Product())
            {
                products = ctx.Products.ToList();
            }

            if(products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet]
        [Route("api/product/get/{id}")]
        public Product Get(int id)
        {
            using(var ctx = new EmployeeDBEntities_Product())
            {
                return ctx.Products.FirstOrDefault(p => p.ProductId == id);
            }
        }

        [HttpPost]
        [Route("api/product/postNewProduct")]
        public IHttpActionResult PostNewProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new EmployeeDBEntities_Product())
            {
                ctx.Products.Add(new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
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

        [Route("api/product/updateProduct")]
        public IHttpActionResult Put(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest("The data you entered is not valid");

            using (var ctx = new EmployeeDBEntities_Product())
            {
                var existingProduct = ctx.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if(existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;

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
        [Route("api/product/deleteProduct/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Not a valid Product Id");

            using (var ctx = new EmployeeDBEntities_Product())
            {
                var product = ctx.Products.FirstOrDefault(p => p.ProductId == id);

                ctx.Entry(product).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
    }
}
