using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebToKindle.Database;
using WebToKindle.Database.Tables;

namespace WebToKindle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookTemplatesController : ControllerBase
    {
        private readonly WebToKindleDB _context;

        public BookTemplatesController(WebToKindleDB context)
        {
            _context = context;
        }

        // GET: api/BookTemplates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookTemplate>>> GetBookTemplates()
        {
            return await _context.BookTemplates.ToListAsync();
        }

        // GET: api/BookTemplates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookTemplate>> GetBookTemplate(int id)
        {
            var bookTemplate = await _context.BookTemplates.FindAsync(id);

            if (bookTemplate == null)
            {
                return NotFound();
            }

            return bookTemplate;
        }

        // PUT: api/BookTemplates/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookTemplate(int id, BookTemplate bookTemplate)
        {
            if (id != bookTemplate.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookTemplate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookTemplateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookTemplates
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BookTemplate>> PostBookTemplate(BookTemplate bookTemplate)
        {
            _context.BookTemplates.Add(bookTemplate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookTemplate", new { id = bookTemplate.Id }, bookTemplate);
        }

        // DELETE: api/BookTemplates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookTemplate>> DeleteBookTemplate(int id)
        {
            var bookTemplate = await _context.BookTemplates.FindAsync(id);
            if (bookTemplate == null)
            {
                return NotFound();
            }

            _context.BookTemplates.Remove(bookTemplate);
            await _context.SaveChangesAsync();

            return bookTemplate;
        }

        private bool BookTemplateExists(int id)
        {
            return _context.BookTemplates.Any(e => e.Id == id);
        }
    }
}
