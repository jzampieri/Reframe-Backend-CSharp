namespace Reframe.Api.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Industry { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}