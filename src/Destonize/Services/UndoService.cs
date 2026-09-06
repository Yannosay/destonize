using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Destonize.Models;

namespace Destonize.Services
{
    public class UndoService
    {
        private readonly string _undoFile;
        private List<UndoEntry> _entries;

        public UndoService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var destonizeDir = Path.Combine(appData, "Destonize");
            Directory.CreateDirectory(destonizeDir);
            _undoFile = Path.Combine(destonizeDir, "undo-log.json");
            _entries = LoadEntries();
        }

        public void AddMove(string source, string destination)
        {
            _entries.Add(new UndoEntry { Source = source, Destination = destination });
            SaveEntries();
        }

        public bool UndoLastMove()
        {
            if (_entries.Count == 0)
                return false;
            var entry = _entries[_entries.Count - 1];
            _entries.RemoveAt(_entries.Count - 1);
            SaveEntries();
            try
            {
                if (File.Exists(entry.Destination))
                {
                    if (File.Exists(entry.Source))
                    {
                        File.Delete(entry.Source);
                    }
                    File.Move(entry.Destination, entry.Source);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public int GetUndoCount() => _entries.Count;

        private List<UndoEntry> LoadEntries()
        {
            if (!File.Exists(_undoFile))
                return new List<UndoEntry>();
            try
            {
                var json = File.ReadAllText(_undoFile);
                return JsonSerializer.Deserialize<List<UndoEntry>>(json) ?? new List<UndoEntry>();
            }
            catch
            {
                return new List<UndoEntry>();
            }
        }

        private void SaveEntries()
        {
            var json = JsonSerializer.Serialize(_entries);
            File.WriteAllText(_undoFile, json);
        }

        private class UndoEntry
        {
            public string Source { get; set; } = string.Empty;
            public string Destination { get; set; } = string.Empty;
        }
    }
}