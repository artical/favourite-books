using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models.ViewModels
{
    public class BooksViewModel
    {
        public int total { get; set; }
        public int page { get; set; }
        public int per_page { get; set; }
        public IEnumerable<Book> books { get; set; }
    }
}
