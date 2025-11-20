namespace Reframe.Api.Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int MarketDemandScore { get; set; }

        public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
    }
}