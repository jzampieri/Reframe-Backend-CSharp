using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserSkillsController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public UserSkillsController(ReframeDbContext context)
        {
            _context = context;
        }

        public class CreateUserSkillDto
        {
            public int UserId { get; set; }
            public int SkillId { get; set; }
            public int CurrentLevel { get; set; }
            public int TargetLevel { get; set; }
        }

        /// <summary>
        /// Cria um vínculo de skill para um usuário (definindo nível atual e alvo).
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UserSkill>> Post([FromBody] CreateUserSkillDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null)
                return NotFound($"Usuário {dto.UserId} não encontrado.");

            var skill = await _context.Skills.FindAsync(dto.SkillId);
            if (skill == null)
                return NotFound($"Skill {dto.SkillId} não encontrada.");

            if (dto.TargetLevel <= dto.CurrentLevel)
                return BadRequest("TargetLevel deve ser maior que CurrentLevel para existir gap.");

            var userSkill = new UserSkill
            {
                UserId = dto.UserId,
                SkillId = dto.SkillId,
                CurrentLevel = dto.CurrentLevel,
                TargetLevel = dto.TargetLevel
            };

            _context.UserSkills.Add(userSkill);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetByUser),
                new { userId = userSkill.UserId, version = "1.0" },
                userSkill
            );
        }

        /// <summary>
        /// Lista todas as skills de um usuário.
        /// </summary>
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<UserSkill>>> GetByUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound($"Usuário {userId} não encontrado.");

            var items = await _context.UserSkills
                .Include(us => us.Skill)
                .Where(us => us.UserId == userId)
                .ToListAsync();

            return Ok(items);
        }
    }
}
