using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LearningPathsController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public LearningPathsController(ReframeDbContext context)
        {
            _context = context;
        }

        [HttpPost("generate/{userId:int}")]
        public async Task<ActionResult<LearningPath>> Generate(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserSkills)
                .ThenInclude(us => us.Skill)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("Usuário não encontrado.");

            var prioritizedSkills = user.UserSkills
                .Select(us => new
                {
                    us.Skill,
                    Gap = us.TargetLevel - us.CurrentLevel,
                    Score = (us.TargetLevel - us.CurrentLevel) * us.Skill.MarketDemandScore
                })
                .Where(x => x.Gap > 0)
                .OrderByDescending(x => x.Score)
                .Take(5)
                .ToList();

            if (!prioritizedSkills.Any())
                return BadRequest("Usuário não possui gaps para gerar trilha.");

            var lp = new LearningPath
            {
                UserId = userId,
                StrategySummary = "Trilha gerada com base em gaps de skill e demanda de mercado."
            };

            int order = 1;
            foreach (var item in prioritizedSkills)
            {
                lp.Tasks.Add(new LearningTask
                {
                    Order = order++,
                    Title = $"Aprofundar em {item.Skill.Name}",
                    Type = "course",
                    Description = "Completar um micro-curso e um projeto prático relacionado à skill."
                });
            }

            _context.LearningPaths.Add(lp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByUser), new { userId = userId, version = "1.0" }, lp);
        }


        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<LearningPath>>> GetByUser(int userId)
        {
            var paths = await _context.LearningPaths
                .Include(lp => lp.Tasks)
                .Where(lp => lp.UserId == userId)
                .ToListAsync();

            return Ok(paths);
        }
    }
}