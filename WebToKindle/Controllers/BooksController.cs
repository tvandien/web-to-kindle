using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebToKindle.Database;
using System.Text.RegularExpressions;
using WebToKindle.Database.Tables;
using WebToKindle.Helper;

namespace WebToKindle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly WebToKindleDB _context;

        public BooksController(WebToKindleDB context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        [HttpGet("create")]
        public async Task<ActionResult<Book>> CreateBook()
        {
            var book = new Book()
            {
                ChapterCount = 1,
                LastUpdate = DateTime.Now,
                Name = "HPMOR",
                IndexURL = "https://m.fanfiction.net/s/5782108/1/Harry-Potter-and-the-Methods-of-Rationality",
                ChapterURL = "https://m.fanfiction.net/s/5782108/{0}/Harry-Potter-and-the-Methods-of-Rationality"
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            var CountType = new RegexType()
            {
                Name = RegexTypes.ChapterCount.ToString(),
                Description = ""
            };
            _context.RegexTypes.Add(CountType);
            _context.SaveChanges();

            var TitleType = new RegexType()
            {
                Name = RegexTypes.ChapterTitle.ToString(),
                Description = ""
            };
            _context.RegexTypes.Add(TitleType);
            _context.SaveChanges();

            var ContentType = new RegexType()
            {
                Name = RegexTypes.ChapterContent.ToString(),
                Description = ""
            };
            _context.RegexTypes.Add(ContentType);
            _context.SaveChanges();

            var regex = new Database.Tables.Regex()
            {
                Book = book,
                RegexString = "Ch 1 of <a href=\'/s/5782108/[0-9]+/\'>([0-9]+)</a",
                Type = CountType,
            };
            _context.Regexes.Add(regex);

            var regex2 = new Database.Tables.Regex()
            {
                Book = book,
                RegexString = "(Chapter .+?)<br></div>",
                Type = TitleType,
            };
            _context.Regexes.Add(regex2);

            var regex3 = new Database.Tables.Regex()
            {
                Book = book,
                RegexString = "<div style='.+?' class='storycontent nocopy' id='storycontent' >(.+?)<div align=center>",
                Type = ContentType,
            };
            _context.Regexes.Add(regex3);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        public class UpdateResult
        {
            public int BookId { get; set; }
            public bool NewChapters { get; set; }
        }


        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
