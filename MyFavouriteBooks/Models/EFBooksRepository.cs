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
        //Task<int> ? IF = 1 ok, if = 0 error?
        public async Task<int> AddBook(Book book, string userId)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == book.ISBN);
            if (dbEntry == null)
            {
               context.Books.Add(book);
            }
            
            context.UserBooks.Add(new UserBook
            {
                ISBN = book.ISBN,
                UserId = userId,

            });
            int result = 0;
            try
            {
                 result = await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return result;
        }
        //skip top10 paginating
        public IEnumerable<Book> GetBooks(string userId)
        {
            return context.UserBooks.Where(x => x.UserId == userId).Select(x=>x.Book).ToList();
        }

        public async Task<int> RemoveBook(string bookId, string userId)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == bookId);
            if (dbEntry != null)
            {
                context.UserBooks.Remove(new UserBook
                {
                    ISBN = bookId,
                    UserId = userId
                });
                return await context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
    }
}
