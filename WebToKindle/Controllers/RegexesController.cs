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
    public class RegexesController : ControllerBase
    {
        private readonly WebToKindleDB _context;

        public RegexesController(WebToKindleDB context)
        {
            _context = context;
        }

        // GET: api/Regexes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regex>>> GetRegexes()
        {
            return await _context.Regexes.ToListAsync();
        }

        // GET: api/Regexes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Regex>> GetRegex(int id)
        {
            var regex = await _context.Regexes.FindAsync(id);

            if (regex == null)
            {
                return NotFound();
            }

            return regex;
        }

        // PUT: api/Regexes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegex(int id, Regex regex)
        {
            if (id != regex.Id)
            {
                return BadRequest();
            }

            _context.Entry(regex).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegexExists(id))
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

        // POST: api/Regexes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Regex>> PostRegex(Regex regex)
        {
            _context.Regexes.Add(regex);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegex", new { id = regex.Id }, regex);
        }

        // DELETE: api/Regexes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Regex>> DeleteRegex(int id)
        {
            var regex = await _context.Regexes.FindAsync(id);
            if (regex == null)
            {
                return NotFound();
            }

            _context.Regexes.Remove(regex);
            await _context.SaveChangesAsync();

            return regex;
        }

        private bool RegexExists(int id)
        {
            return _context.Regexes.Any(e => e.Id == id);
        }
    }
}
