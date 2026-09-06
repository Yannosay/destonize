using System;
using System.Diagnostics;
using System.Windows;
using Destonize.Services;

namespace Destonize
{
    public partial class UpdateAvailableWindow : Window
    {
        private readonly string _latestVersion;
        private readonly string _updateSource;
        private readonly SettingsService _settingsService;

        public UpdateAvailableWindow(string latestVersion, string updateSource, SettingsService settingsService)
        {
            InitializeComponent();
            _latestVersion = latestVersion;
            _updateSource = updateSource;
            _settingsService = settingsService;
            UpdateInfoText.Text = $"Destonize {_latestVersion} is available. You are currently on an older version.";
            Loaded += (s, e) => DarkTitleBarHelper.ApplyDarkTitleBar(this);
        }

        private void UpdateNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _updateSource.Replace("api.", "").Replace("repos/", "").Replace("/releases/latest", "/releases/latest"),
                    UseShellExecute = true
                });
            }
            catch
            {
                DarkMessageBox.Show("Could not open browser.", "Error");
            }
            DialogResult = true;
            Close();
        }

        private void Remind_Click(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.GetSettings();
            settings.UpdateRemindAt = DateTime.Now.AddDays(1);
            _settingsService.SaveSettings(settings);
            DialogResult = true;
            Close();
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            var settings = _settingsService.GetSettings();
            settings.SkippedUpdateVersion = _latestVersion;
            _settingsService.SaveSettings(settings);
            DialogResult = true;
            Close();
        }
    }
}