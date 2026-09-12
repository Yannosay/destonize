$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$orgCs = Join-Path $root 'src\Destonize\OrganizeWindow.xaml.cs'

$orgCsContent = @'
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Destonize.Models;
using Destonize.Services;
using Microsoft.Win32;

namespace Destonize
{
    public partial class OrganizeWindow : Window
    {
        private readonly SettingsService _settingsService;
        private readonly ObservableCollection<SortingRule> _rules;
        private SortingRule? _selectedRule;
        private List<string> _categories;

        public ObservableCollection<SortingRule> Rules => _rules;

        public SortingRule? SelectedRule
        {
            get => _selectedRule;
            set
            {
                if (_selectedRule == value) return;
                _selectedRule = value;
                if (value != null)
                {
                    PopulateFieldsFromRule(value);
                }
                else
                {
                    ClearFormFields();
                }
            }
        }

        public OrganizeWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;
            _rules = new ObservableCollection<SortingRule>(_settingsService.GetSettings().SortingRules);
            _categories = _settingsService.GetSettings().FileCategories.Keys.ToList();
            _categories.Insert(0, "None");

            if (_categories.Count == 1)
            {
                var defaults = new Dictionary<string, List<string>>
                {
                    { "Images", new List<string> { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".svg", ".webp" } },
                    { "Videos", new List<string> { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv" } },
                    { "Documents", new List<string> { ".pdf", ".doc", ".docx", ".txt", ".rtf", ".xls", ".xlsx", ".ppt", ".pptx" } },
                    { "Music", new List<string> { ".mp3", ".wav", ".flac", ".aac", ".ogg" } },
                    { "Archives", new List<string> { ".zip", ".rar", ".7z", ".tar", ".gz" } },
                    { "Programs", new List<string> { ".exe", ".msi", ".bat", ".cmd", ".ps1" } },
                    { "Other", new List<string>() }
                };
                foreach (var kvp in defaults)
                    _categories.Add(kvp.Key);
                var settings = _settingsService.GetSettings();
                settings.FileCategories = defaults;
                _settingsService.SaveSettings(settings);
            }

            RuleCategoryBox.ItemsSource = _categories;
            DataContext = this;
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);

            foreach (var rule in _rules)
            {
                rule.ConditionSummary = BuildConditionSummary(rule);
            }
        }

        private string BuildConditionSummary(SortingRule rule)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(rule.Pattern)) parts.Add($"Pattern: {rule.Pattern}");
            if (!string.IsNullOrWhiteSpace(rule.NameFilter)) parts.Add($"Name: {rule.NameFilter}");
            if (!string.IsNullOrWhiteSpace(rule.ExtensionCategory)) parts.Add($"Category: {rule.ExtensionCategory}");
            if (rule.MinSizeMB.HasValue) parts.Add($"Size ≥ {rule.MinSizeMB}MB");
            if (rule.MaxSizeMB.HasValue) parts.Add($"Size ≤ {rule.MaxSizeMB}MB");
            if (rule.MinAgeDays.HasValue) parts.Add($"Age ≥ {rule.MinAgeDays}d");
            if (rule.MaxAgeDays.HasValue) parts.Add($"Age ≤ {rule.MaxAgeDays}d");
            if (rule.ExtensionInclusions.Count > 0) parts.Add($"Incl: {string.Join(",", rule.ExtensionInclusions)}");
            if (rule.ExtensionExclusions.Count > 0) parts.Add($"Excl: {string.Join(",", rule.ExtensionExclusions)}");
            if (rule.NameExclusions.Count > 0) parts.Add($"Name excl: {string.Join(",", rule.NameExclusions)}");
            return parts.Count > 0 ? string.Join(" | ", parts) : "No conditions";
        }

        private void PopulateFieldsFromRule(SortingRule rule)
        {
            RuleNameBox.Text = rule.Name;
            RulePatternBox.Text = rule.Pattern;
            RuleDestinationBox.Text = rule.Destination;
            RuleNameFilterBox.Text = rule.NameFilter ?? string.Empty;
            SetCategorySelection(rule.ExtensionCategory);
            MinSizeBox.Text = rule.MinSizeMB?.ToString() ?? string.Empty;
            MaxSizeBox.Text = rule.MaxSizeMB?.ToString() ?? string.Empty;
            MinAgeBox.Text = rule.MinAgeDays?.ToString() ?? string.Empty;
            MaxAgeBox.Text = rule.MaxAgeDays?.ToString() ?? string.Empty;
            NameExclusionsBox.Text = string.Join(", ", rule.NameExclusions);
            ExtExclusionsBox.Text = string.Join(", ", rule.ExtensionExclusions);
            ExtInclusionsBox.Text = string.Join(", ", rule.ExtensionInclusions);
        }

        private void ClearFormFields()
        {
            RuleNameBox.Text = string.Empty;
            RulePatternBox.Text = string.Empty;
            RuleDestinationBox.Text = string.Empty;
            RuleNameFilterBox.Text = string.Empty;
            RuleCategoryBox.SelectedIndex = 0;
            MinSizeBox.Text = string.Empty;
            MaxSizeBox.Text = string.Empty;
            MinAgeBox.Text = string.Empty;
            MaxAgeBox.Text = string.Empty;
            NameExclusionsBox.Text = string.Empty;
            ExtExclusionsBox.Text = string.Empty;
            ExtInclusionsBox.Text = string.Empty;
        }

        private void SetCategorySelection(string? category)
        {
            if (string.IsNullOrEmpty(category))
            {
                RuleCategoryBox.SelectedIndex = 0;
            }
            else
            {
                int index = _categories.IndexOf(category);
                RuleCategoryBox.SelectedIndex = index >= 0 ? index : 0;
            }
        }

        private string? GetSelectedCategory()
        {
            if (RuleCategoryBox.SelectedIndex == 0) return null;
            return RuleCategoryBox.SelectedItem as string;
        }

        private List<string> ParseList(string text)
        {
            return text.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Select(s => s.Trim())
                       .Where(s => !string.IsNullOrEmpty(s))
                       .ToList();
        }

        private double? ParseDouble(string text)
        {
            if (double.TryParse(text, out double result))
                return result;
            return null;
        }

        private int? ParseInt(string text)
        {
            if (int.TryParse(text, out int result))
                return result;
            return null;
        }

        private SortingRule BuildRuleFromForm()
        {
            return new SortingRule
            {
                Name = RuleNameBox.Text.Trim(),
                Pattern = RulePatternBox.Text.Trim(),
                Destination = RuleDestinationBox.Text.Trim(),
                NameFilter = string.IsNullOrWhiteSpace(RuleNameFilterBox.Text) ? null : RuleNameFilterBox.Text.Trim(),
                ExtensionCategory = GetSelectedCategory(),
                MinSizeMB = ParseDouble(MinSizeBox.Text),
                MaxSizeMB = ParseDouble(MaxSizeBox.Text),
                MinAgeDays = ParseInt(MinAgeBox.Text),
                MaxAgeDays = ParseInt(MaxAgeBox.Text),
                NameExclusions = ParseList(NameExclusionsBox.Text),
                ExtensionExclusions = ParseList(ExtExclusionsBox.Text),
                ExtensionInclusions = ParseList(ExtInclusionsBox.Text),
                IsEnabled = true
            };
        }

        private bool ValidateRule(SortingRule rule, bool showErrors = true)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(rule.Name))
                errors.Add("Rule name is required.");
            if (string.IsNullOrWhiteSpace(rule.Destination))
                errors.Add("Destination folder is required.");
            if (string.IsNullOrWhiteSpace(rule.Pattern) &&
                string.IsNullOrWhiteSpace(rule.NameFilter) &&
                string.IsNullOrWhiteSpace(rule.ExtensionCategory) &&
                !rule.MinSizeMB.HasValue && !rule.MaxSizeMB.HasValue &&
                !rule.MinAgeDays.HasValue && !rule.MaxAgeDays.HasValue &&
                rule.ExtensionInclusions.Count == 0)
                errors.Add("At least one condition is required (Pattern, Name filter, Category, Size, Age, or Extension Inclusions).");

            if (rule.MinSizeMB.HasValue && rule.MaxSizeMB.HasValue && rule.MinSizeMB > rule.MaxSizeMB)
                errors.Add("Minimum size cannot be larger than maximum size.");
            if (rule.MinAgeDays.HasValue && rule.MaxAgeDays.HasValue && rule.MinAgeDays > rule.MaxAgeDays)
                errors.Add("Minimum age cannot be larger than maximum age.");

            if (showErrors && errors.Count > 0)
                DarkMessageBox.Show(string.Join(Environment.NewLine, errors), "Invalid Rule");
            return errors.Count == 0;
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var rule = BuildRuleFromForm();
            if (!ValidateRule(rule)) return;

            rule.ConditionSummary = BuildConditionSummary(rule);
            _rules.Add(rule);
            ClearFormFields();
            _selectedRule = null;
            RulesListView.SelectedItem = null;
        }

        private void UpdateRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null)
            {
                DarkMessageBox.Show("Please select a rule to update.", "No Selection");
                return;
            }
            var updated = BuildRuleFromForm();
            if (!ValidateRule(updated)) return;

            updated.ConditionSummary = BuildConditionSummary(updated);
            updated.IsEnabled = SelectedRule.IsEnabled;
            int index = _rules.IndexOf(SelectedRule);
            _rules[index] = updated;
            SelectedRule = updated;
            RulesListView.Items.Refresh();
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null)
            {
                DarkMessageBox.Show("Please select a rule to delete.", "No Selection");
                return;
            }

            var confirm = new DarkConfirmBox($"Are you sure you want to delete the rule \"{SelectedRule.Name}\"?", "Confirm Delete");
            if (confirm.ShowDialog() == true)
            {
                _rules.Remove(SelectedRule);
                // The binding will automatically set SelectedRule to null, which clears the fields.
            }
        }

        private void RulesListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && SelectedRule != null)
            {
                RemoveRule_Click(sender, e);
                e.Handled = true;
            }
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            _selectedRule = null;
            RulesListView.SelectedItem = null;
            ClearFormFields();
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            int index = _rules.IndexOf(SelectedRule);
            if (index > 0)
            {
                _rules.Move(index, index - 1);
                RulesListView.SelectedIndex = index - 1;
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            int index = _rules.IndexOf(SelectedRule);
            if (index < _rules.Count - 1)
            {
                _rules.Move(index, index + 1);
                RulesListView.SelectedIndex = index + 1;
            }
        }

        private void DuplicateRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            var clone = new SortingRule
            {
                Name = SelectedRule.Name + " (Copy)",
                Pattern = SelectedRule.Pattern,
                Destination = SelectedRule.Destination,
                NameFilter = SelectedRule.NameFilter,
                MinSizeMB = SelectedRule.MinSizeMB,
                MaxSizeMB = SelectedRule.MaxSizeMB,
                MinAgeDays = SelectedRule.MinAgeDays,
                MaxAgeDays = SelectedRule.MaxAgeDays,
                ExtensionCategory = SelectedRule.ExtensionCategory,
                NameExclusions = new List<string>(SelectedRule.NameExclusions),
                ExtensionExclusions = new List<string>(SelectedRule.ExtensionExclusions),
                ExtensionInclusions = new List<string>(SelectedRule.ExtensionInclusions),
                IsEnabled = SelectedRule.IsEnabled
            };
            clone.ConditionSummary = BuildConditionSummary(clone);
            _rules.Add(clone);
            SelectedRule = clone;
        }

        private void TestRule_Click(object sender, RoutedEventArgs e)
        {
            var rule = BuildRuleFromForm();
            if (!ValidateRule(rule, false))
            {
                DarkMessageBox.Show("Please fix the rule before testing.", "Invalid Rule");
                return;
            }
            var service = new FileOrganizerService(_settingsService);
            int count = service.CountMatches(rule);
            DarkMessageBox.Show($"The rule matches {count} file(s) on the desktop.", "Test Result");
        }

        private void BrowseDestination_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Destination Folder"
            };
            if (dialog.ShowDialog() == true)
            {
                RuleDestinationBox.Text = dialog.FolderName;
            }
        }

        private List<string> ValidateAllRules()
        {
            var warnings = new List<string>();
            foreach (var rule in _rules)
            {
                if (string.IsNullOrWhiteSpace(rule.Destination))
                {
                    warnings.Add($"Rule '{rule.Name}' has no destination folder and will be skipped.");
                    continue;
                }
                if (Directory.Exists(rule.Destination) && rule.Destination.Equals(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), StringComparison.OrdinalIgnoreCase))
                    warnings.Add($"Rule '{rule.Name}' points to the Desktop itself. Files may not be moved.");
                if (!rule.IsEnabled)
                    warnings.Add($"Rule '{rule.Name}' is disabled and will be ignored.");
            }
            return warnings;
        }

        private void OrganizeNow_Click(object sender, RoutedEventArgs e)
        {
            if (_rules.Count == 0)
            {
                DarkMessageBox.Show("No rules defined. Please add at least one rule.", "No Rules");
                return;
            }

            var warnings = ValidateAllRules();
            if (warnings.Count > 0)
            {
                var warnBox = new DarkConfirmBox(
                    "Warnings:\n" + string.Join("\n", warnings) + "\n\nDo you want to continue?",
                    "Warnings Found");
                if (warnBox.ShowDialog() != true)
                    return;
            }

            var service = new FileOrganizerService(_settingsService);
            int totalMatches = 0;
            foreach (var rule in _rules.Where(r => r.IsEnabled))
            {
                try
                {
                    totalMatches += service.CountMatches(rule);
                }
                catch (Exception ex)
                {
                    DarkMessageBox.Show($"Failed to count matches for rule '{rule.Name}': {ex.Message}", "Error");
                    return;
                }
            }

            var confirmMessage = $"Approximately {totalMatches} file(s) will be processed.\n\nDo you want to organize the desktop now?";
            var confirm = new DarkConfirmBox(confirmMessage, "Confirm Organization");
            if (confirm.ShowDialog() != true)
                return;

            try
            {
                var settings = _settingsService.GetSettings();
                settings.SortingRules = _rules.ToList();
                _settingsService.SaveSettings(settings);

                service.OrganizeAll();
                DarkMessageBox.Show("Desktop organized successfully.", "Destonize");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                DarkMessageBox.Show($"Organization failed: {ex.Message}", "Error");
            }
        }

        private void LoadSchema_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Destonize Schema (*.ize)|*.ize|All files (*.*)|*.*",
                Title = "Load Organize Schema"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var json = File.ReadAllText(dialog.FileName);
                    var rules = System.Text.Json.JsonSerializer.Deserialize<List<SortingRule>>(json);
                    if (rules != null)
                    {
                        _rules.Clear();
                        foreach (var rule in rules)
                        {
                            rule.ConditionSummary = BuildConditionSummary(rule);
                            _rules.Add(rule);
                        }
                        ClearFormFields();
                        _selectedRule = null;
                        RulesListView.SelectedItem = null;
                        DarkMessageBox.Show("Schema loaded successfully.", "Success");
                    }
                }
                catch (Exception ex)
                {
                    DarkMessageBox.Show($"Failed to load schema: {ex.Message}", "Error");
                }
            }
        }

        private void SaveSchema_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Destonize Schema (*.ize)|*.ize|All files (*.*)|*.*",
                Title = "Save Organize Schema"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(_rules.ToList(), new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(dialog.FileName, json);
                    DarkMessageBox.Show("Schema saved successfully.", "Success");
                }
                catch (Exception ex)
                {
                    DarkMessageBox.Show($"Failed to save schema: {ex.Message}", "Error");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AdvancedOptionsCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            bool isAdvanced = AdvancedOptionsCheckBox.IsChecked == true;
            ConditionsGroupBox.Visibility = isAdvanced ? Visibility.Visible : Visibility.Collapsed;
            ExclusionsGroupBox.Visibility = isAdvanced ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
'@

Set-Content -Path $orgCs -Value $orgCsContent -Encoding UTF8