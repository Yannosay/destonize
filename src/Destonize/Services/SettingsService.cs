using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Destonize.Models;

namespace Destonize.Services
{
    public class SettingsService
    {
        private readonly string _configDir;
        private readonly string _configFile;
        private readonly string _stateFile;
        private AppSettings? _settings;

        public SettingsService()
        {
            _configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Destonize");
            _configFile = Path.Combine(_configDir, "config.json");
            _stateFile = Path.Combine(_configDir, "state.json");
            Directory.CreateDirectory(_configDir);
            if (!File.Exists(_configFile))
            {
                var defaultSettings = new AppSettings();
                InitializeDefaultCategories(defaultSettings);
                SaveSettings(defaultSettings);
            }
            if (!File.Exists(_stateFile))
            {
                var initialState = new { FirstRun = true, WelcomeAccepted = false };
                File.WriteAllText(_stateFile, JsonSerializer.Serialize(initialState));
            }
        }

        public AppSettings GetSettings()
        {
            if (_settings == null)
            {
                var json = File.ReadAllText(_configFile);
                _settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            return _settings;
        }

        public void SaveSettings(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_configFile, json);
            _settings = settings;
        }

        public bool IsFirstRun()
        {
            var json = File.ReadAllText(_stateFile);
            var state = JsonSerializer.Deserialize<State>(json);
            return state?.FirstRun ?? true;
        }

        public void SetFirstRunComplete()
        {
            var state = new State { FirstRun = false, WelcomeAccepted = true };
            File.WriteAllText(_stateFile, JsonSerializer.Serialize(state));
        }

        private void InitializeDefaultCategories(AppSettings settings)
        {
            settings.FileCategories["Images"] = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };
            settings.FileCategories["Documents"] = new List<string> { ".doc", ".docx", ".pdf", ".txt", ".rtf", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".ods" };
            settings.FileCategories["Audio"] = new List<string> { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma", ".m4a" };
            settings.FileCategories["Video"] = new List<string> { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm" };
            settings.FileCategories["Archives"] = new List<string> { ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2" };
            settings.FileCategories["Code"] = new List<string> { ".cs", ".js", ".ts", ".py", ".java", ".cpp", ".h", ".html", ".css", ".json", ".xml", ".yml", ".yaml" };
            settings.FileCategories["Executables"] = new List<string> { ".exe", ".msi", ".bat", ".cmd", ".ps1", ".apk" };
            settings.FileCategories["Other"] = new List<string>();
        }

        private class State
        {
            public bool FirstRun { get; set; }
            public bool WelcomeAccepted { get; set; }
        }
    }
}