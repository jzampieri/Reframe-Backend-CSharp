using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public UsersController(ReframeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await _context.Users
                .Include(u => u.Organization)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _context.Users
                .Include(u => u.Organization)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id, version = "1.0" }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] User updated)
        {
            if (id != updated.Id)
                return BadRequest("Id do path diferente do body.");

            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.FullName = updated.FullName;
            existing.Email = updated.Email;
            existing.Role = updated.Role;
            existing.OrganizationId = updated.OrganizationId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound();

            _context.Users.Remove(existing);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}