using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public SkillsController(ReframeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> Get()
        {
            var skills = await _context.Skills.ToListAsync();
            return Ok(skills);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Skill>> GetById(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        [HttpPost]
        public async Task<ActionResult<Skill>> Post([FromBody] Skill skill)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = skill.Id, version = "1.0" }, skill);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Skill updated)
        {
            if (id != updated.Id)
                return BadRequest("Id do path diferente do body.");

            var existing = await _context.Skills.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = updated.Name;
            existing.Category = updated.Category;
            existing.MarketDemandScore = updated.MarketDemandScore;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Skills.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.Skills.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}