using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Destonize.Models;

namespace Destonize.Services
{
    public class FileOrganizerService
    {
        private readonly SettingsService _settingsService;
        private readonly UndoService _undoService;

        public FileOrganizerService(SettingsService settingsService)
        {
            _settingsService = settingsService;
            _undoService = new UndoService();
        }

        public void OrganizeAll()
        {
            var settings = _settingsService.GetSettings();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var files = Directory.GetFiles(desktopPath);
            var rules = settings.SortingRules.Where(r => r.IsEnabled).ToList();

            foreach (var file in files)
            {
                if (settings.SkipHiddenFiles && (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;
                if (settings.SkipSystemFiles && (File.GetAttributes(file) & FileAttributes.System) == FileAttributes.System)
                    continue;

                foreach (var rule in rules)
                {
                    if (MatchRule(file, rule))
                    {
                        MoveFile(file, rule.Destination, settings.CreateUndoLog);
                        break;
                    }
                }
            }
        }

        public int CountMatches(SortingRule rule)
        {
            var settings = _settingsService.GetSettings();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var files = Directory.GetFiles(desktopPath);
            int count = 0;
            foreach (var file in files)
            {
                if (settings.SkipHiddenFiles && (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden)
                    continue;
                if (settings.SkipSystemFiles && (File.GetAttributes(file) & FileAttributes.System) == FileAttributes.System)
                    continue;
                if (MatchRule(file, rule))
                    count++;
            }
            return count;
        }

        private bool MatchRule(string filePath, SortingRule rule)
        {
            var fileName = Path.GetFileName(filePath);
            var ext = Path.GetExtension(filePath).ToLowerInvariant();
            var fi = new FileInfo(filePath);

            bool hasAnyCondition = false;

            if (!string.IsNullOrWhiteSpace(rule.Pattern))
            {
                hasAnyCondition = true;
                bool patternMatch;
                if (rule.Pattern.StartsWith("*."))
                {
                    var targetExt = rule.Pattern.Substring(1).ToLowerInvariant();
                    patternMatch = ext == targetExt;
                }
                else
                {
                    patternMatch = fileName.Contains(rule.Pattern, StringComparison.OrdinalIgnoreCase);
                }
                if (!patternMatch) return false;
            }

            if (!string.IsNullOrWhiteSpace(rule.NameFilter))
            {
                hasAnyCondition = true;
                if (!fileName.Contains(rule.NameFilter, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(rule.ExtensionCategory))
            {
                hasAnyCondition = true;
                var settings = _settingsService.GetSettings();
                if (settings.FileCategories.TryGetValue(rule.ExtensionCategory, out var categoryExtensions))
                {
                    if (!categoryExtensions.Contains(ext))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (rule.ExtensionInclusions.Count > 0)
            {
                hasAnyCondition = true;
                if (!rule.ExtensionInclusions.Any(e => string.Equals(e.TrimStart('.'), ext.TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
                    return false;
            }

            if (rule.ExtensionExclusions.Count > 0)
            {
                if (rule.ExtensionExclusions.Any(e => string.Equals(e.TrimStart('.'), ext.TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
                    return false;
            }

            if (rule.NameExclusions.Count > 0)
            {
                if (rule.NameExclusions.Any(ex => fileName.Contains(ex, StringComparison.OrdinalIgnoreCase)))
                    return false;
            }

            if (rule.MinSizeMB.HasValue)
            {
                hasAnyCondition = true;
                var sizeMB = fi.Length / (1024.0 * 1024.0);
                if (sizeMB < rule.MinSizeMB.Value) return false;
            }

            if (rule.MaxSizeMB.HasValue)
            {
                hasAnyCondition = true;
                var sizeMB = fi.Length / (1024.0 * 1024.0);
                if (sizeMB > rule.MaxSizeMB.Value) return false;
            }

            if (rule.MinAgeDays.HasValue)
            {
                hasAnyCondition = true;
                var age = DateTime.Now - fi.LastWriteTime;
                if (age.TotalDays < rule.MinAgeDays.Value) return false;
            }

            if (rule.MaxAgeDays.HasValue)
            {
                hasAnyCondition = true;
                var age = DateTime.Now - fi.LastWriteTime;
                if (age.TotalDays > rule.MaxAgeDays.Value) return false;
            }

            if (!hasAnyCondition)
                return false;

            return true;
        }

        private void MoveFile(string filePath, string destination, bool createUndoLog)
        {
            var fileName = Path.GetFileName(filePath);
            var destDir = destination;
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);
            var destFile = Path.Combine(destDir, fileName);
            if (File.Exists(destFile))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var newName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
                destFile = Path.Combine(destDir, newName);
            }
            File.Move(filePath, destFile);
            if (createUndoLog)
            {
                _undoService.AddMove(filePath, destFile);
            }
        }
    }
}