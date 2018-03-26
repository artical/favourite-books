using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class EFBookRepository : IBookRepository
    {
        private ApplicationDbContext context;
        public EFBookRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }
        public async Task AddBook(Book book)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == book.ISBN);
            if (dbEntry == null)
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
            }
        }

        public Book GetBook(string isbn)
        {
            return context.Books.FirstOrDefault(x => x.ISBN == isbn);
        }
    }
}
