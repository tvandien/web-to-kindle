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
    public class MailHistoriesController : ControllerBase
    {
        private readonly WebToKindleDB _context;

        public MailHistoriesController(WebToKindleDB context)
        {
            _context = context;
        }

        // GET: api/MailHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MailHistory>>> GetMailHistory()
        {
            return await _context.MailHistory.ToListAsync();
        }

        // GET: api/MailHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MailHistory>> GetMailHistory(int id)
        {
            var mailHistory = await _context.MailHistory.FindAsync(id);

            if (mailHistory == null)
            {
                return NotFound();
            }

            return mailHistory;
        }

        // PUT: api/MailHistories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMailHistory(int id, MailHistory mailHistory)
        {
            if (id != mailHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(mailHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MailHistoryExists(id))
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

        // POST: api/MailHistories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MailHistory>> PostMailHistory(MailHistory mailHistory)
        {
            _context.MailHistory.Add(mailHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMailHistory", new { id = mailHistory.Id }, mailHistory);
        }

        // DELETE: api/MailHistories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MailHistory>> DeleteMailHistory(int id)
        {
            var mailHistory = await _context.MailHistory.FindAsync(id);
            if (mailHistory == null)
            {
                return NotFound();
            }

            _context.MailHistory.Remove(mailHistory);
            await _context.SaveChangesAsync();

            return mailHistory;
        }

        private bool MailHistoryExists(int id)
        {
            return _context.MailHistory.Any(e => e.Id == id);
        }
    }
}
