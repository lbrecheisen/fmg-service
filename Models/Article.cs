using Fmg.Models.Common;

namespace Fmg.Models
{
    public class Article : Entity
    {
        public string AgentId { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}