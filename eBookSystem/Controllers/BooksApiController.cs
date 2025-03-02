using Microsoft.AspNetCore.Mvc;
using eBookSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace eBookSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly Database _database;

        public BooksApiController(Database database)
        {
            _database = database;
        }

        [HttpGet("spotlight")]
        public ActionResult<IEnumerable<Book>> GetSpotlightBooks()
        {
            var books = _database.GetAllBooks().Take(4).ToList(); // Get top 4 books for spotlight
            return Ok(books);
        }

        [HttpGet("newarrivals")]
        public ActionResult<IEnumerable<Book>> GetNewArrivals()
        {
            var books = _database.GetAllBooks().OrderByDescending(b => b.PublishedYear).Take(4).ToList(); // Get latest 4 books for new arrivals
            return Ok(books);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            var books = _database.GetAllBooks();
            return Ok(books);
        }
    }
}
