using Fmg.Models.Common;

namespace Fmg.Models
{
    public class Lead : Entity
    {
        public string AgentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? MaxPrice { get; set; }
        public string? MinBedrooms { get; set; }
        public string? MinBathrooms { get; set; }
        public string? MinGarageSpaces { get; set; }
        public string? MinSquareFootage { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}