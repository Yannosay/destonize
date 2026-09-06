using System;
using System.Collections.Generic;

namespace Destonize.Models
{
    public class AppSettings
    {
        public bool AutoUpdate { get; set; } = true;
        public bool MinimizeToTray { get; set; } = false;
        public bool StartWithWindows { get; set; } = false;
        public bool ConfirmBeforeOrganize { get; set; } = true;
        public bool SkipHiddenFiles { get; set; } = true;
        public bool SkipSystemFiles { get; set; } = true;
        public bool CreateUndoLog { get; set; } = true;
        public bool ShowSplash { get; set; } = true;
        public string UpdateSource { get; set; } = "https://api.github.com/repos/Yannosay/Destonize/releases/latest";
        public string SkippedUpdateVersion { get; set; } = string.Empty;
        public DateTime? UpdateRemindAt { get; set; } = null;
        public List<SortingRule> SortingRules { get; set; } = new List<SortingRule>();
        public Dictionary<string, List<string>> FileCategories { get; set; } = new Dictionary<string, List<string>>();
    }
}