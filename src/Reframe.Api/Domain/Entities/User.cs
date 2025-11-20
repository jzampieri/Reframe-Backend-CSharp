using Reframe.Api.Domain.Enums;

namespace Reframe.Api.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;

        public int? OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
        public ICollection<LearningPath> LearningPaths { get; set; } = new List<LearningPath>();
    }
}