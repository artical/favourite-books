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
        public async Task<int> AddBook(Book book, string userId)
        {
            var dbEntry = context.Books.FirstOrDefault(x => x.ISBN == book.ISBN);
            if (dbEntry == null)
            {
               context.Books.Add(book);
            }

            var userBook = context.UserBooks.FirstOrDefault(x => x.ISBN == book.ISBN && x.UserId == userId);
            if (userBook != null)
            {
                throw new Exception("already exists");
            }
            userBook = new UserBook
            {
                ISBN = book.ISBN,
                UserId = userId,
                Added = DateTime.Now
            };
            context.UserBooks.Add(userBook);
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
            return context.UserBooks.Where(x => x.UserId == userId).OrderByDescending(x=>x.Added).Select(x=>x.Book).ToList();
        }

        public async Task<int> RemoveBook(string bookId, string userId)
        {
            var dbEntry = context.UserBooks.FirstOrDefault(x => x.ISBN == bookId && x.UserId == userId);
            if (dbEntry != null)
            {
                context.UserBooks.Remove(dbEntry);
                int results = await context.SaveChangesAsync();
                return results;
            }
            else
            {
                return 0;
            }
        }

        public UserBook GetBook(string bookId, string userId)
        {
            return context.UserBooks.FirstOrDefault(x => x.ISBN == bookId && x.UserId == userId);
        }
    }
}
