using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public interface IBookRepository
    {
        Task AddBook(Book book);
        Book GetBook(string isbn); 
    }
}
