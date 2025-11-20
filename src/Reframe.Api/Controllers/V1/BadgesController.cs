using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BadgesController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public BadgesController(ReframeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Badge>> Create([FromBody] Badge badge)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Badges.Add(badge);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = badge.Id, version = "1.0" }, badge);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Badge>> GetById(int id)
        {
            var badge = await _context.Badges.FindAsync(id);
            if (badge == null)
                return NotFound();

            return Ok(badge);
        }

        [HttpPost("award/user/{userId:int}/badge/{badgeId:int}")]
        public async Task<ActionResult<UserBadge>> Award(int userId, int badgeId)
        {
            var user = await _context.Users.FindAsync(userId);
            var badge = await _context.Badges.FindAsync(badgeId);

            if (user == null || badge == null)
                return NotFound("Usuário ou badge não encontrado.");

            var ub = new UserBadge
            {
                UserId = userId,
                BadgeId = badgeId,
                AwardedAt = DateTime.UtcNow
            };

            _context.UserBadges.Add(ub);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserBadges), new { userId = userId, version = "1.0" }, ub);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<UserBadge>>> GetUserBadges(int userId)
        {
            var list = await _context.UserBadges
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId)
                .ToListAsync();

            return Ok(list);
        }
    }
}