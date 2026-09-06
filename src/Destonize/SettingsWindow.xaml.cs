using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Destonize.Models;
using Destonize.Services;

namespace Destonize
{
    public partial class SettingsWindow : Window
    {
        private readonly SettingsService _settingsService;
        private AppSettings _settings;

        public SettingsWindow(SettingsService settingsService)
        {
            InitializeComponent();
            _settingsService = settingsService;
            _settings = _settingsService.GetSettings();
            LoadSettings();
            LoadSystemInfo();
            LoadCategories();
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void LoadSettings()
        {
            AutoUpdateCheckBox.IsChecked = _settings.AutoUpdate;
            MinimizeToTrayCheckBox.IsChecked = _settings.MinimizeToTray;
            StartWithWindowsCheckBox.IsChecked = StartupService.IsStartupEnabled();
            ConfirmBeforeOrganizeCheckBox.IsChecked = _settings.ConfirmBeforeOrganize;
            ShowSplashCheckBox.IsChecked = _settings.ShowSplash;
            SkipHiddenFilesCheckBox.IsChecked = _settings.SkipHiddenFiles;
            SkipSystemFilesCheckBox.IsChecked = _settings.SkipSystemFiles;
            CreateUndoLogCheckBox.IsChecked = _settings.CreateUndoLog;
            UpdateSourceBox.Text = _settings.UpdateSource;
        }

        private void LoadSystemInfo()
        {
            var sysInfo = SystemInfoService.GetSystemInfo();
            ComputerNameText.Text = sysInfo.ComputerName;
            OSText.Text = sysInfo.OS;
            CPUText.Text = sysInfo.CPU;
            MemoryText.Text = sysInfo.Memory;
            PowerShellText.Text = sysInfo.PowerShell;
        }

        private void LoadCategories()
        {
            CategoriesListBox.ItemsSource = _settings.FileCategories.Keys.ToList();
            if (CategoriesListBox.Items.Count > 0)
                CategoriesListBox.SelectedIndex = 0;
        }

        private void CategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is string category && _settings.FileCategories.ContainsKey(category))
            {
                ExtensionsListBox.ItemsSource = _settings.FileCategories[category];
            }
            else
            {
                ExtensionsListBox.ItemsSource = null;
            }
        }

        private void AddExtension_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is string category && _settings.FileCategories.ContainsKey(category))
            {
                var ext = NewExtensionBox.Text.Trim();
                if (string.IsNullOrEmpty(ext))
                    return;
                if (!ext.StartsWith("."))
                    ext = "." + ext;
                if (!_settings.FileCategories[category].Contains(ext))
                {
                    _settings.FileCategories[category].Add(ext);
                    ExtensionsListBox.ItemsSource = null;
                    ExtensionsListBox.ItemsSource = _settings.FileCategories[category];
                }
                NewExtensionBox.Text = string.Empty;
            }
        }

        private void RemoveExtension_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is string category &&
                _settings.FileCategories.ContainsKey(category) &&
                ExtensionsListBox.SelectedItem is string ext)
            {
                _settings.FileCategories[category].Remove(ext);
                ExtensionsListBox.ItemsSource = null;
                ExtensionsListBox.ItemsSource = _settings.FileCategories[category];
            }
        }

        private void NewCategory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new DarkInputBox("Enter new category name:", "New Category");
            if (dialog.ShowDialog() == true)
            {
                string newName = dialog.InputText.Trim();
                if (!string.IsNullOrEmpty(newName) && !_settings.FileCategories.ContainsKey(newName))
                {
                    _settings.FileCategories[newName] = new List<string>();
                    LoadCategories();
                    CategoriesListBox.SelectedItem = newName;
                }
            }
        }

        private void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesListBox.SelectedItem is string category && _settings.FileCategories.ContainsKey(category))
            {
                var confirm = new DarkConfirmBox($"Delete category '{category}' and its extensions?", "Confirm Delete");
                if (confirm.ShowDialog() == true)
                {
                    _settings.FileCategories.Remove(category);
                    LoadCategories();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _settings.AutoUpdate = AutoUpdateCheckBox.IsChecked ?? false;
            _settings.MinimizeToTray = MinimizeToTrayCheckBox.IsChecked ?? false;
            _settings.ConfirmBeforeOrganize = ConfirmBeforeOrganizeCheckBox.IsChecked ?? false;
            _settings.ShowSplash = ShowSplashCheckBox.IsChecked ?? false;
            _settings.SkipHiddenFiles = SkipHiddenFilesCheckBox.IsChecked ?? false;
            _settings.SkipSystemFiles = SkipSystemFilesCheckBox.IsChecked ?? false;
            _settings.CreateUndoLog = CreateUndoLogCheckBox.IsChecked ?? false;
            _settings.UpdateSource = UpdateSourceBox.Text.Trim();
            _settingsService.SaveSettings(_settings);

            try
            {
                StartupService.SetStartup(StartWithWindowsCheckBox.IsChecked ?? false);
            }
            catch (System.Exception ex)
            {
                DarkMessageBox.Show("Failed to set startup: " + ex.Message, "Error");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}