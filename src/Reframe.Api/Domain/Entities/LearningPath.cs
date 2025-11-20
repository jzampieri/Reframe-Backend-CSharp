namespace Reframe.Api.Domain.Entities
{
    public class LearningPath
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string StrategySummary { get; set; } = string.Empty;

        public ICollection<LearningTask> Tasks { get; set; } = new List<LearningTask>();
    }

    public class LearningTask
    {
        public int Id { get; set; }
        public int LearningPathId { get; set; }
        public LearningPath LearningPath { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = "course";
        public int Order { get; set; }
        public bool Completed { get; set; } = false;
    }
}