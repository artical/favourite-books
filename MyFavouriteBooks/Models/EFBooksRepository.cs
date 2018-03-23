using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class EFBooksRepository : IBooksRepository
    {
        private ApplicationDbContext context;
        public EFBooksRepository(ApplicationDbContext _context)
        {
            this.context = _context;
        }
        public async Task AddBook(Book book)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == book.ISBN);
            if (dbEntry == null)
            {
                await context.Books.AddAsync(book);
            }

            await context.UserBooks.AddAsync(new UserBook
            {
                ISBN = book.ISBN,
                UserId = "1"
            });
            return;
        }

        public IEnumerable<Book> GetBooks(string userId)
        {
            return context.UserBooks.Where(x => x.UserId == userId).Select(x=>x.Book);
        }

        public async Task RemoveBook(Book book)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == book.ISBN);
            if (dbEntry != null)
            {
                context.UserBooks.Remove(new UserBook
                {
                    ISBN = book.ISBN,
                    UserId = "1"
                });
                await context.SaveChangesAsync();
            }
            else
            {
                ///error
            }
        }
    }
}
