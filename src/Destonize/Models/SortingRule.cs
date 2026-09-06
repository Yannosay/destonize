using System.Collections.Generic;

namespace Destonize.Models
{
    public class SortingRule
    {
        public string Name { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string? NameFilter { get; set; }
        public double? MinSizeMB { get; set; }
        public double? MaxSizeMB { get; set; }
        public int? MinAgeDays { get; set; }
        public int? MaxAgeDays { get; set; }
        public string? ExtensionCategory { get; set; }
        public List<string> NameExclusions { get; set; } = new List<string>();
        public List<string> ExtensionExclusions { get; set; } = new List<string>();
        public List<string> ExtensionInclusions { get; set; } = new List<string>();
        public string ConditionSummary { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
    }
}