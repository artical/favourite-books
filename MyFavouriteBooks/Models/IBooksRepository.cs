using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public interface IBooksRepository
    {
        Task AddBook(Book book);
        Task RemoveBook(Book book);
        IEnumerable<Book> GetBooks(string userId);
    }
}
