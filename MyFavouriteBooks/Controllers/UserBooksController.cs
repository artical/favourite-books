using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavouriteBooks.Models;
using MyFavouriteBooks.Models.ViewModels;

namespace MyFavouriteBooks.Controllers
{
    [Route("api/user/books")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserBooksController : Controller
    {
        private IUserBooksRepository repository;
        public UserBooksController(IUserBooksRepository _repository)
        {
            this.repository = _repository;
        }
        /// <summary>
        /// Retrives user favourite books with pagination
        /// </summary>
        /// <param name="page">Current page number</param>
        /// <param name="per_page">Books per page</param>
        /// <param name="query">Search query</param>
        /// <response code="200">Product created</response>
        /// <response code="400">Missing/invalid values in query</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(BooksViewModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult Get(int page, int per_page, string query)
        {
            string claimId = ParseUserIdFromClaims(User);
            if (String.IsNullOrEmpty(claimId))
            {
                return BadRequest("Can't get user Id");
            }

            var books = repository.GetBooks(claimId);
            if (!String.IsNullOrEmpty(query))
                books = books.Where(x => x.Title.ToLower().Contains(query.ToLower())).ToList();
            var response = new
            {
                total =  books.Count(),
                page,
                per_page,
                books = books.Skip((page-1) * per_page).Take(per_page).ToList()
            };
            return Ok(response);
        }
        /// <summary>
        /// Get a user book from favourite list by ISBN
        /// </summary>
        /// <param name="isbn">isbn of the book</param>
        [HttpGet("{isbn}")]
        [ProducesResponseType(typeof(UserBook), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public IActionResult Get(string isbn)
        {
            string claimId = ParseUserIdFromClaims(User);
            if (String.IsNullOrEmpty(claimId))
            {
                return BadRequest("Can't get user Id");
            }
            var userBook = repository.GetBook(isbn, claimId);
            return Ok(userBook);
        }
        /// <summary>
        /// Add a new user book to favourite list
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
        public async Task<IActionResult> Post([FromBody]Book book)
        {
            string claimId = ParseUserIdFromClaims(User);
            if (String.IsNullOrEmpty(claimId))
            {
                return BadRequest("Can't get user Id");
            }
            await repository.AddBook(book, claimId);
            return Ok();
        }
        /// <summary>
        /// Remove a book from favourite list of the user
        /// </summary>
        /// <param name="isbn">isbn of a book</param>
        /// <returns></returns>
        /// <response code="200">Book removed from the list</response>
        /// <response code="400">Missing/invalid values</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpDelete("{isbn}")]

        public async Task<IActionResult> Delete([FromRoute] string isbn)
        {
            string claimId = ParseUserIdFromClaims(User);
            if (String.IsNullOrEmpty(claimId))
            {
                return BadRequest("Can't get user Id");
            }
            await repository.RemoveBook(isbn, claimId);
            return Ok();
        }

        private static string ParseUserIdFromClaims(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
        }
    }
}
