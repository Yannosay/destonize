using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using System.Linq;
using Destonize.Services;

namespace Destonize.ViewModels
{
    public class MainViewModel
    {
        private readonly SettingsService _settingsService;
        private ObservableCollection<FileItem> _allFiles = new();

        public ObservableCollection<FileItem> Files { get; } = new();
        public ObservableCollection<FileItem> FilteredFiles { get; } = new();

        public MainViewModel(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void LoadDesktopFiles()
        {
            _allFiles.Clear();
            Files.Clear();
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            foreach (var file in Directory.GetFiles(desktopPath))
            {
                var fi = new FileInfo(file);
                var item = new FileItem
                {
                    Name = fi.Name,
                    FullPath = file,
                    SizeFormatted = FormatFileSize(fi.Length),
                    LastModified = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm"),
                    Type = GetFileType(fi.Extension),
                    Icon = GetFileIcon(fi.Extension)
                };
                _allFiles.Add(item);
                Files.Add(item);
            }
            FilteredFiles.Clear();
            foreach (var file in _allFiles)
                FilteredFiles.Add(file);
            ApplyGrouping();
        }

        public void Filter(string searchText)
        {
            FilteredFiles.Clear();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var file in _allFiles)
                    FilteredFiles.Add(file);
            }
            else
            {
                foreach (var file in _allFiles.Where(f => f.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                    FilteredFiles.Add(file);
            }
            ApplyGrouping();
        }

        private void ApplyGrouping()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(FilteredFiles);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(FileItem.Type)));
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes >= 1024 * 1024 * 1024)
                return string.Format("{0:F2} GB", bytes / (1024.0 * 1024 * 1024));
            if (bytes >= 1024 * 1024)
                return string.Format("{0:F2} MB", bytes / (1024.0 * 1024));
            if (bytes >= 1024)
                return string.Format("{0:F2} KB", bytes / 1024.0);
            return bytes + " B";
        }

        private string GetFileType(string extension)
        {
            extension = extension.ToLowerInvariant();
            switch (extension)
            {
                case ".jpg": case ".jpeg": case ".png": case ".gif": case ".bmp": case ".tiff": case ".webp":
                    return "Image";
                case ".doc": case ".docx": case ".pdf": case ".txt": case ".rtf": case ".xls": case ".xlsx":
                case ".ppt": case ".pptx": case ".odt": case ".ods":
                    return "Document";
                case ".mp3": case ".wav": case ".flac": case ".aac": case ".ogg": case ".wma": case ".m4a":
                    return "Audio";
                case ".mp4": case ".avi": case ".mkv": case ".mov": case ".wmv": case ".flv": case ".webm":
                    return "Video";
                case ".zip": case ".rar": case ".7z": case ".tar": case ".gz": case ".bz2":
                    return "Archive";
                case ".cs": case ".js": case ".ts": case ".py": case ".java": case ".cpp": case ".h":
                case ".html": case ".css": case ".json": case ".xml": case ".yml": case ".yaml":
                    return "Code";
                case ".exe": case ".msi": case ".bat": case ".cmd": case ".ps1": case ".apk":
                    return "Executable";
                default:
                    return "Other";
            }
        }

        private string GetFileIcon(string extension)
        {
            extension = extension.ToLowerInvariant();
            switch (extension)
            {
                case ".jpg": case ".jpeg": case ".png": case ".gif": case ".bmp": case ".tiff": case ".webp":
                    return "🖼️";
                case ".doc": case ".docx": case ".pdf": case ".txt": case ".rtf": case ".xls": case ".xlsx":
                case ".ppt": case ".pptx": case ".odt": case ".ods":
                    return "📄";
                case ".mp3": case ".wav": case ".flac": case ".aac": case ".ogg": case ".wma": case ".m4a":
                    return "🎵";
                case ".mp4": case ".avi": case ".mkv": case ".mov": case ".wmv": case ".flv": case ".webm":
                    return "🎬";
                case ".zip": case ".rar": case ".7z": case ".tar": case ".gz": case ".bz2":
                    return "📦";
                case ".cs": case ".js": case ".ts": case ".py": case ".java": case ".cpp": case ".h":
                case ".html": case ".css": case ".json": case ".xml": case ".yml": case ".yaml":
                    return "💻";
                case ".exe": case ".msi": case ".bat": case ".cmd": case ".ps1": case ".apk":
                    return "⚙️";
                default:
                    return "📄";
            }
        }
    }

    public class FileItem
    {
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string SizeFormatted { get; set; } = string.Empty;
        public string LastModified { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}