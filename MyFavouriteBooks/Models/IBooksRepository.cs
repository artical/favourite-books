using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public interface IBooksRepository
    {
        void AddBook(Book book);
        void RemoveBook(Book book);
        IEnumerable<Book> GetBooks();
    }
}
