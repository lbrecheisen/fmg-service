using Fmg.Models.Common;

namespace Fmg.Models
{
    public class Agent : Entity
    {
        public string Oid { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }
}