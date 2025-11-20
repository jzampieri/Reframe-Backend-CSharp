using Microsoft.EntityFrameworkCore;
using Reframe.Api.Domain.Entities;

namespace Reframe.Api.Infrastructure
{
    public class ReframeDbContext : DbContext
    {
        public ReframeDbContext(DbContextOptions<ReframeDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<UserSkill> UserSkills => Set<UserSkill>();
        public DbSet<LearningPath> LearningPaths => Set<LearningPath>();
        public DbSet<LearningTask> LearningTasks => Set<LearningTask>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<UserBadge> UserBadges => Set<UserBadge>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationId);

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSkills)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSkill>()
                .HasOne(us => us.Skill)
                .WithMany(s => s.UserSkills)
                .HasForeignKey(us => us.SkillId);

            modelBuilder.Entity<LearningTask>()
                .HasOne(t => t.LearningPath)
                .WithMany(lp => lp.Tasks)
                .HasForeignKey(t => t.LearningPathId);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBadges)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.Badge)
                .WithMany(b => b.UserBadges)
                .HasForeignKey(ub => ub.BadgeId);
        }
    }
}