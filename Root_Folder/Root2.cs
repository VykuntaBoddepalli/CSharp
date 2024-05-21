using System;
using System.Linq;
using System.Web.Http;
using Vidly_MVCProject.Dtos;
using Vidly_MVCProject.Models;

namespace Vidly_MVCProject.Controllers.Api
{
    public class ProductsController : ApiController
    {
        private AppDbContext _appDbContext;

        public ProductsController()
        {
            _appDbContext = new AppDbContext();
        }

        // GET /api/products
        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            var products = _appDbContext.Products.ToList();
            return Ok(products);
        }

        // GET /api/products/1
        [HttpGet]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _appDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST /api/products
        [HttpPost]
        public IHttpActionResult CreateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                DateAdded = DateTime.Now
            };

            _appDbContext.Products.Add(product);
            _appDbContext.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + product.Id), product);
        }

        // PUT /api/products/1
        [HttpPut]
        public IHttpActionResult UpdateProduct(int id, ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var productInDb = _appDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (productInDb == null)
                return NotFound();

            productInDb.Name = productDto.Name;
            productInDb.Price = productDto.Price;
            productInDb.CategoryId = productDto.CategoryId;
            _appDbContext.SaveChanges();

            return Ok();
        }

        // DELETE /api/products/1
        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            var productInDb = _appDbContext.Products.SingleOrDefault(p => p.Id == id);
            if (productInDb == null)
                return NotFound();

            _appDbContext.Products.Remove(productInDb);
            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
