using System;
using System.Linq;
using System.Web.Http;
using Vidly_MVCProject.Dtos;
using Vidly_MVCProject.Models;

namespace Vidly_MVCProject.Controllers.Api
{
    public class IndependentController : ApiController
    {
        private AppDbContext _appDbContext;

        public IndependentController()
        {
            _appDbContext = new AppDbContext();
        }

        //GET /api/independent/customers
        [HttpGet]
        public IHttpActionResult GetCustomers()
        {
            var customers = _appDbContext.Customers.ToList();
            return Ok(customers);
        }

        //GET /api/independent/movies
        [HttpGet]
        public IHttpActionResult GetMovies()
        {
            var movies = _appDbContext.Movies.ToList();
            return Ok(movies);
        }

        //POST /api/independent/customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = new Customer
            {
                Name = customerDto.Name,
                IsSubscribedToNewsletter = customerDto.IsSubscribedToNewsletter,
                MembershipTypeId = customerDto.MembershipTypeId,
                Birthdate = customerDto.Birthdate
            };

            _appDbContext.Customers.Add(customer);
            _appDbContext.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customer);
        }

        //POST /api/independent/movie
        [HttpPost]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = new Movie
            {
                Name = movieDto.Name,
                GenreId = movieDto.GenreId,
                ReleaseDate = movieDto.ReleaseDate,
                DateAdded = DateTime.Now,
                NumberInStock = movieDto.NumberInStock,
                NumberAvailable = movieDto.NumberAvailable
            };

            _appDbContext.Movies.Add(movie);
            _appDbContext.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movie);
        }

        //POST /api/independent/rental
        [HttpPost]
        public IHttpActionResult CreateRental(NewRentalDto newRental)
        {
            var customer = _appDbContext.Customers.SingleOrDefault(
                c => c.Id == newRental.CustomerId);

            if (customer == null)
                return BadRequest("CustomerId is not valid.");

            var movies = _appDbContext.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRental.MovieIds.Count)
                return BadRequest("One or more MovieIds are invalid.");

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie is not available");

                movie.NumberAvailable--;

                var rental = new Rental()
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _appDbContext.Rentals.Add(rental);
            }

            _appDbContext.SaveChanges();

            return Ok();
        }
    }
}
