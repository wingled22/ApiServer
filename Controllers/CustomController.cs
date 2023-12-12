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
    public class CustomController : ControllerBase
    {
        private readonly OlappContext _context;

        public CustomController(OlappContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            return await _context.Clients.ToListAsync();
        }

        [HttpPost("PostClient")]
        public IActionResult PostClient(ClientAndLoanCreation c){
            return Ok();
        } 

        // // GET: api/Client/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Client>> GetClient(long id)
        // {
        //   if (_context.Clients == null)
        //   {
        //       return NotFound();
        //   }
        //     var client = await _context.Clients.FindAsync(id);

        //     if (client == null)
        //     {
        //         return NotFound();
        //     }

        //     return client;
        // }

        // // PUT: api/Client/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutClient(long id, Client client)
        // {
        //     if (id != client.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(client).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ClientExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Client
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Client>> PostClient(Client client)
        // {
        //   if (_context.Clients == null)
        //   {
        //       return Problem("Entity set 'OlappContext.Clients'  is null.");
        //   }
        //     _context.Clients.Add(client);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetClient", new { id = client.Id }, client);
        // }

        // // DELETE: api/Client/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteClient(long id)
        // {
        //     if (_context.Clients == null)
        //     {
        //         return NotFound();
        //     }
        //     var client = await _context.Clients.FindAsync(id);
        //     if (client == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Clients.Remove(client);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ClientExists(long id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
