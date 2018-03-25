using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Ignore<User>();
            modelBuilder.Entity<UserBook>().HasKey(u => new { u.ISBN, u.UserId });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
    }
}
