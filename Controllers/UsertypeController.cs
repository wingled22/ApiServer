using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using olappApi.Entities;

namespace olappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsertypeController : ControllerBase
    {
        private readonly OlappContext _context;

        public UsertypeController(OlappContext context)
        {
            _context = context;
        }

        // GET: api/Usertype
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usertype>>> GetUsertypes()
        {
          if (_context.Usertypes == null)
          {
              return NotFound();
          }
            return await _context.Usertypes.ToListAsync();
        }

        // GET: api/Usertype/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usertype>> GetUsertype(int id)
        {
          if (_context.Usertypes == null)
          {
              return NotFound();
          }
            var usertype = await _context.Usertypes.FindAsync(id);

            if (usertype == null)
            {
                return NotFound();
            }

            return usertype;
        }

        // PUT: api/Usertype/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsertype(int id, Usertype usertype)
        {
            if (id != usertype.Id)
            {
                return BadRequest();
            }

            _context.Entry(usertype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsertypeExists(id))
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

        // POST: api/Usertype
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usertype>> PostUsertype(Usertype usertype)
        {
          if (_context.Usertypes == null)
          {
              return Problem("Entity set 'OlappContext.Usertypes'  is null.");
          }
            _context.Usertypes.Add(usertype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsertype", new { id = usertype.Id }, usertype);
        }

        // DELETE: api/Usertype/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsertype(int id)
        {
            if (_context.Usertypes == null)
            {
                return NotFound();
            }
            var usertype = await _context.Usertypes.FindAsync(id);
            if (usertype == null)
            {
                return NotFound();
            }

            _context.Usertypes.Remove(usertype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsertypeExists(int id)
        {
            return (_context.Usertypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
