using Library.Data;
using Library.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            return true;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b => b.Author).Include(b => b.CheckedOutTo).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Book> GetByIdAsync(int id)
        {
            return (await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id)) ?? new Book();
        }

        [HttpPost]
        public async Task<bool> AddAsync(Book book)
        {
            if(book.Author == null)
            {
                return false;
            }

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == book.Author.Id);

            if (author == null)
            {
                return false;
            }

            book.Author = author;

            if (book.Id == 0)
            {
                await _context.Books.AddAsync(book);
            }
            else
            {
                _context.Update(book);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
