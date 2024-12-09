using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bitsEFClasses.Models;

namespace bitsRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchesController : ControllerBase
    {
        private readonly BitsContext _context;

        public BatchesController(BitsContext context)
        {
            _context = context;
        }

        // GET: api/Batches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Object>>> GetBatches()
        {
          if (_context.Batches == null)
          {
              return NotFound();
          }
            return await  _context.Batches.Select(b => 
                new { b.BatchId, b.RecipeId, b.Recipe.Name, b.Volume, b.Recipe.RecipeIngredients, b.ScheduledStartDate}).ToListAsync();
                         
        }

        // GET: api/Batches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetBatch(int id)
        {
          if (_context.Batches == null)
          {
              return NotFound();
          }
            var batch = await _context.Batches.Select(b =>
                new { b.BatchId, b.RecipeId, b.Recipe.Name, b.Volume, b.Recipe.RecipeIngredients, b.ScheduledStartDate }).Where(b => b.BatchId == id).SingleOrDefaultAsync();

            if (batch == null)
            {
                return NotFound();
            }

            return batch;
        }

        // PUT: api/Batches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatch(int id, Batch batch)
        {
            if (id != batch.BatchId)
            {
                return BadRequest();
            }

            _context.Entry(batch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExists(id))
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

        // POST: api/Batches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Batch>> PostBatch(Batch batch)
        {
          if (_context.Batches == null)
          {
              return Problem("Entity set 'BitsContext.Batches'  is null.");
          }
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBatch", new { id = batch.BatchId }, batch);
        }

        // DELETE: api/Batches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            if (_context.Batches == null)
            {
                return NotFound();
            }
            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            _context.Batches.Remove(batch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BatchExists(int id)
        {
            return (_context.Batches?.Any(e => e.BatchId == id)).GetValueOrDefault();
        }
    }
}
