using Library.Data;
using Library.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public CheckOutController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<CheckedOut>> GetAllAsync()
        {
            return await _context.Checkouts.Include(c => c.Book).Include(c => c.Member).Where(c => c.ReturnedDate == null).ToListAsync();
        }

        [HttpPost]
        public async Task<bool> CheckOutBook(CheckedOut checkedOut)
        {
            var existingCheckOuts = await _context.Checkouts.Where(c => c.Book.Id == checkedOut.Book.Id && c.ReturnedDate == null).ToListAsync();
            if (existingCheckOuts != null && existingCheckOuts.Any())
            {
                foreach (var checkOut in existingCheckOuts)
                {
                    checkOut.ReturnedDate = DateTime.Now;
                }
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == checkedOut.Book.Id);

            if (book == null)
            {
                return false;
            }

            var member = await _context.Members.FirstOrDefaultAsync(b => b.Id == checkedOut.Member.Id);
            book.CheckedOutTo = member;

            var newCheckedOut = new CheckedOut();
            newCheckedOut.Book = book;
            newCheckedOut.Member = member;
            newCheckedOut.CheckedOutDate = DateTime.Now;

            await _context.Checkouts.AddAsync(newCheckedOut);
                        
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        [Route("checkin")]
        public async Task<bool> CheckInBook([FromBody] int bookId)
        {
            var existingCheckOut = await _context.Checkouts.FirstOrDefaultAsync(c => c.Book.Id == bookId && c.ReturnedDate == null);
            if (existingCheckOut == null)
            {
                return false;
            }

            var book = _context.Books.Include(b => b.CheckedOutTo).FirstOrDefault(b => b.Id == bookId);

            if (book == null)
            {
                return false;
            }

            book.CheckedOutTo = null;

            existingCheckOut.ReturnedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
