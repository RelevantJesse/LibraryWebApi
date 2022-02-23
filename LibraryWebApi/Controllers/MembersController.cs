using Library.Data;
using Library.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public MembersController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _context.Members.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Member> GetByIdAsync(int id)
        {
            return (await _context.Members.FirstOrDefaultAsync(b => b.Id == id)) ?? new Member();
        }

        [HttpPost]
        public async Task<bool> AddAsync(Member member)
        {
            if (member == null)
            {
                return false;
            }

            if (member.Id == 0)
            {
                await _context.Members.AddAsync(member);
            }
            else
            {
                _context.Update(member);
            }
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
