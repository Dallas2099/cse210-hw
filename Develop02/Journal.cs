using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Develop02
{
    /// <summary>
    /// Manages the collection of journal entries and handles persistence.
    /// </summary>
    public class Journal
    {
        private readonly List<Entry> _entries = new List<Entry>();
        private readonly string _delimiter;

        public Journal(string delimiter)
        {
            _delimiter = delimiter;
        }

        public void AddEntry(Entry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            _entries.Add(entry);
        }

        public IEnumerable<Entry> GetEntries()
        {
            return _entries.ToList();
        }

        public void Display()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("The journal is empty.");
                return;
            }

            foreach (var entry in _entries)
            {
                Console.WriteLine(entry.ToDisplayString());
                Console.WriteLine();
            }
        }

        public void SaveToFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be empty.", nameof(filePath));
            }

            var lines = _entries.Select(entry => entry.ToStorageString(_delimiter));
            File.WriteAllLines(filePath, lines);
        }

        public void LoadFromFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file was not found.", filePath);
            }

            var lines = File.ReadAllLines(filePath);
            _entries.Clear();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                _entries.Add(Entry.FromStorageString(line, _delimiter));
            }
        }
    }
}
