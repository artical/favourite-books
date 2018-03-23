using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFavouriteBooks.Models;

namespace MyFavouriteBooks.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBooksRepository repository;
        BooksController (IBooksRepository _repository)
        {
            this.repository = _repository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
