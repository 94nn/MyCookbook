using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCookbook.API.Data;
using MyCookbook.API.Models;

namespace MyCookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DishesController : ControllerBase
    {
        private readonly CookbookDbContext _context;

        public DishesController(CookbookDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetAll()
        {
            // 1. Fetch the dishes and their steps cleanly from the DB
            var dishes = await _context.Dishes
                .Include(d => d.Steps)
                .ToListAsync();

            // 2. Sort the steps in memory for each dish before returning
            foreach (var dish in dishes)
            {
                dish.Steps = dish.Steps.OrderBy(s => s.StepNumber).ToList();
            }

            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetById(int id)
        {
            var dish = await _context.Dishes
                .Include(d => d.Steps)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dish == null)
            {
                return NotFound();
            }

            // Sort the steps for this single dish
            dish.Steps = dish.Steps.OrderBy(s => s.StepNumber).ToList();

            return Ok(dish);
        }

        [HttpPost]
        public async Task<ActionResult<Dish>> Create(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            var existingDish = await _context.Dishes
                .Include(d => d.Steps)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (existingDish == null)
            {
                return NotFound();
            }

            // Update dish properties
            existingDish.Name = dish.Name;
            existingDish.Description = dish.Description;
            existingDish.Category = dish.Category;

            // Replace all steps
            _context.DishSteps.RemoveRange(existingDish.Steps);

            if (dish.Steps != null && dish.Steps.Any())
            {
                foreach (var step in dish.Steps)
                {
                    step.DishId = id;
                    step.Id = 0; // Force new IDs — don't reuse old ones
                    _context.DishSteps.Add(step);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Show the actual error message
                return StatusCode(500, new { message = "Database error", error = ex.InnerException?.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}