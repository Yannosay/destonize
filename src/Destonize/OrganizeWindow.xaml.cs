using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
                _selectedRule = value;
                if (value != null)
                {
                    RuleNameBox.Text = value.Name;
                    RulePatternBox.Text = value.Pattern;
                    RuleDestinationBox.Text = value.Destination;
                    RuleNameFilterBox.Text = value.NameFilter ?? string.Empty;
                    SetCategorySelection(value.ExtensionCategory);
                    MinSizeBox.Text = value.MinSizeMB?.ToString() ?? string.Empty;
                    MaxSizeBox.Text = value.MaxSizeMB?.ToString() ?? string.Empty;
                    MinAgeBox.Text = value.MinAgeDays?.ToString() ?? string.Empty;
                    MaxAgeBox.Text = value.MaxAgeDays?.ToString() ?? string.Empty;
                    NameExclusionsBox.Text = string.Join(", ", value.NameExclusions);
                    ExtExclusionsBox.Text = string.Join(", ", value.ExtensionExclusions);
                    ExtInclusionsBox.Text = string.Join(", ", value.ExtensionInclusions);
                }
                else
                {
                    ClearDetails();
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
            if (!string.IsNullOrWhiteSpace(rule.Pattern)) parts.Add(rule.Pattern);
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

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {
            var rule = BuildRuleFromForm();
            if (string.IsNullOrWhiteSpace(rule.Name) || string.IsNullOrWhiteSpace(rule.Destination))
            {
                DarkMessageBox.Show("Name and Destination are required.", "Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(rule.Pattern) &&
                string.IsNullOrWhiteSpace(rule.NameFilter) &&
                rule.ExtensionCategory == null &&
                !rule.MinSizeMB.HasValue && !rule.MaxSizeMB.HasValue &&
                !rule.MinAgeDays.HasValue && !rule.MaxAgeDays.HasValue &&
                rule.ExtensionInclusions.Count == 0)
            {
                DarkMessageBox.Show("At least one condition is required (Pattern, Name filter, Category, Size, Age, or Extension Inclusions).", "Error");
                return;
            }

            rule.ConditionSummary = BuildConditionSummary(rule);
            _rules.Add(rule);
            ClearDetails();
        }

        private void UpdateRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            var updated = BuildRuleFromForm();
            updated.ConditionSummary = BuildConditionSummary(updated);
            updated.IsEnabled = SelectedRule.IsEnabled;
            int index = _rules.IndexOf(SelectedRule);
            _rules[index] = updated;
            SelectedRule = updated;
            RulesListView.Items.Refresh();
        }

        private void RemoveRule_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule != null)
            {
                _rules.Remove(SelectedRule);
                ClearDetails();
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            int index = _rules.IndexOf(SelectedRule);
            if (index > 0)
            {
                _rules.Move(index, index - 1);
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRule == null) return;
            int index = _rules.IndexOf(SelectedRule);
            if (index < _rules.Count - 1)
            {
                _rules.Move(index, index + 1);
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
            if (string.IsNullOrWhiteSpace(rule.Destination))
            {
                DarkMessageBox.Show("Destination is required for testing.", "Error");
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

        private void OrganizeNow_Click(object sender, RoutedEventArgs e)
        {
            if (_settingsService.GetSettings().ConfirmBeforeOrganize)
            {
                var confirm = new DarkConfirmBox("Do you want to organize the desktop now?\nThis will move files according to your rules.", "Confirm Organization");
                if (confirm.ShowDialog() != true)
                    return;
            }

            var settings = _settingsService.GetSettings();
            settings.SortingRules = _rules.ToList();
            _settingsService.SaveSettings(settings);

            var organizer = new FileOrganizerService(_settingsService);
            organizer.OrganizeAll();
            DarkMessageBox.Show("Desktop organized successfully.", "Destonize");
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ClearDetails()
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
            _selectedRule = null;
            RulesListView.SelectedItem = null;
        }
    }
}