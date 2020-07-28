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
    public class RegexTypesController : ControllerBase
    {
        private readonly WebToKindleDB _context;

        public RegexTypesController(WebToKindleDB context)
        {
            _context = context;
        }

        // GET: api/RegexTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegexType>>> GetRegexTypes()
        {
            return await _context.RegexTypes.ToListAsync();
        }

        // GET: api/RegexTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegexType>> GetRegexType(int id)
        {
            var regexType = await _context.RegexTypes.FindAsync(id);

            if (regexType == null)
            {
                return NotFound();
            }

            return regexType;
        }

        // PUT: api/RegexTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegexType(int id, RegexType regexType)
        {
            if (id != regexType.Id)
            {
                return BadRequest();
            }

            _context.Entry(regexType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegexTypeExists(id))
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

        // POST: api/RegexTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RegexType>> PostRegexType(RegexType regexType)
        {
            _context.RegexTypes.Add(regexType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegexType", new { id = regexType.Id }, regexType);
        }

        // DELETE: api/RegexTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RegexType>> DeleteRegexType(int id)
        {
            var regexType = await _context.RegexTypes.FindAsync(id);
            if (regexType == null)
            {
                return NotFound();
            }

            _context.RegexTypes.Remove(regexType);
            await _context.SaveChangesAsync();

            return regexType;
        }

        private bool RegexTypeExists(int id)
        {
            return _context.RegexTypes.Any(e => e.Id == id);
        }
    }
}
