namespace Reframe.Api.Domain.Entities
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int XpValue { get; set; }

        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    }

    public class UserBadge
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int BadgeId { get; set; }
        public Badge Badge { get; set; } = null!;

        public DateTime AwardedAt { get; set; } = DateTime.UtcNow;
    }
}