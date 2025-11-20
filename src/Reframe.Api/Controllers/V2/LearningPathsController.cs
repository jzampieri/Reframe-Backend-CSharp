using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reframe.Api.Infrastructure;

namespace Reframe.Api.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LearningPathsController : ControllerBase
    {
        private readonly ReframeDbContext _context;

        public LearningPathsController(ReframeDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<object>>> GetByUserV2(int userId)
        {
            var paths = await _context.LearningPaths
                .Include(lp => lp.Tasks)
                .Where(lp => lp.UserId == userId)
                .ToListAsync();

            var result = paths.Select(lp => new
            {
                lp.Id,
                lp.UserId,
                lp.CreatedAt,
                lp.StrategySummary,
                TotalTasks = lp.Tasks.Count,
                Tasks = lp.Tasks.Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Type,
                    t.Order,
                    t.Completed,
                    EstimatedHours = 4
                })
            });

            return Ok(result);
        }
    }
}