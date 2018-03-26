using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public interface IBooksRepository
    {
        Task<int> AddBook(Book book, string userId);
        Task<int> RemoveBook(string bookId, string userId);
        IEnumerable<Book> GetBooks(string userId);
        UserBook GetBook(string bookId, string userId);
    }
}
