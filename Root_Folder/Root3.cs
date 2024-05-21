using System;
using System.Linq;
using System.Web.Http;
using Vidly_MVCProject.Dtos;
using Vidly_MVCProject.Models;

namespace Vidly_MVCProject.Controllers.Api
{
    public class OrdersController : ApiController
    {
        private AppDbContext _appDbContext;

        public OrdersController()
        {
            _appDbContext = new AppDbContext();
        }

        // GET /api/orders
        [HttpGet]
        public IHttpActionResult GetOrders()
        {
            var orders = _appDbContext.Orders.ToList();
            return Ok(orders);
        }

        // GET /api/orders/1
        [HttpGet]
        public IHttpActionResult GetOrder(int id)
        {
            var order = _appDbContext.Orders.SingleOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST /api/orders
        [HttpPost]
        public IHttpActionResult CreateOrder(OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                DateOrdered = DateTime.Now,
                TotalAmount = orderDto.TotalAmount
            };

            _appDbContext.Orders.Add(order);
            _appDbContext.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + order.Id), order);
        }

        // PUT /api/orders/1
        [HttpPut]
        public IHttpActionResult UpdateOrder(int id, OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var orderInDb = _appDbContext.Orders.SingleOrDefault(o => o.Id == id);
            if (orderInDb == null)
                return NotFound();

            orderInDb.CustomerId = orderDto.CustomerId;
            orderInDb.TotalAmount = orderDto.TotalAmount;
            _appDbContext.SaveChanges();

            return Ok();
        }

        // DELETE /api/orders/1
        [HttpDelete]
        public IHttpActionResult DeleteOrder(int id)
        {
            var orderInDb = _appDbContext.Orders.SingleOrDefault(o => o.Id == id);
            if (orderInDb == null)
                return NotFound();

            _appDbContext.Orders.Remove(orderInDb);
            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
