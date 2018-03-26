using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models.ViewModels
{
    public class BooksViewModel
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Per_page { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
