using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFavouriteBooks.Models;

namespace MyFavouriteBooks.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BooksController : Controller
    {
        private IBooksRepository repository;
        public BooksController (IBooksRepository _repository)
        {
            this.repository = _repository;
        }

        /// <summary>
        /// Лфлфлф
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            string claimId = User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;///int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value);
            return repository.GetBooks(claimId);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Book book)
        {
            repository.AddBook(book);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete([FromBody]Book book)
        {
            repository.RemoveBook(book);
        }
    }
}
