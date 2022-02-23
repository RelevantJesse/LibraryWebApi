using Library.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CheckedOut> Checkouts { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(b => b.Genre).HasConversion<int>();

            modelBuilder.Entity<Author>()
                .Property(a => a.FirstName).HasMaxLength(50);

            modelBuilder.Entity<Author>()
                .Property(a => a.LastName).HasMaxLength(50);

            modelBuilder.Entity<Book>()
                .Property(a => a.Title).HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(a => a.ISBN).HasMaxLength(30);

            modelBuilder.Entity<Member>()
                .Property(a => a.FirstName).HasMaxLength(50);

            modelBuilder.Entity<Member>()
                .Property(a => a.LastName).HasMaxLength(50);
        }
    }
}
