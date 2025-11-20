namespace Reframe.Api.Domain.Entities
{
    public class UserSkill
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        public int CurrentLevel { get; set; }
        public int TargetLevel { get; set; }
    }
}