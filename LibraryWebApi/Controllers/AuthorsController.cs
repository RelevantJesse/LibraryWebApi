using Library.Data;
using Library.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AuthorsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Author> GetByIdAsync(int id)
        {
            return (await _context.Authors.FirstOrDefaultAsync(b => b.Id == id)) ?? new Author();
        }

        [HttpPost]
        public async Task<bool> AddAsync(Author author)
        {
            if (author == null)
            {
                return false;
            }

            if (author.Id == 0)
            {
                await _context.Authors.AddAsync(author);
            }
            else
            {
                _context.Update(author);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
