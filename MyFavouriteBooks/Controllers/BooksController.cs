using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavouriteBooks.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyFavouriteBooks.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public BooksController(IBookRepository _repository)
        {
            this.repository = _repository;
        }

        /// <summary>
        /// Add a new book to database
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <response code="200">Book added to the list</response>
        /// <response code="400">Missing/invalid values</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            await repository.AddBook(book);
            return Ok();
        }
        /// <summary>
        /// Get book by ISBN
        /// </summary>
        /// <param name="isbn">ISBN of a book</param>
        /// <returns></returns>
        /// <response code="200">Book added to the list</response>
        /// <response code="400">Missing/invalid values</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{isbn}")]
        [ProducesResponseType(typeof(Book), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult Get(string isbn)
        {
            var book = repository.GetBook(isbn);
            return Ok(book);
        }
    }
}
