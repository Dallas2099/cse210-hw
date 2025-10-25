using System;

namespace Develop02
{
    /// <summary>
    /// Represents a single journal entry with a prompt, the user's response, and the date recorded.
    /// </summary>
    public class Entry
    {
        public string Prompt { get; }
        public string Response { get; }
        public string Date { get; }

        public Entry(string prompt, string response, string date)
        {
            Prompt = prompt ?? string.Empty;
            Response = response ?? string.Empty;
            Date = date ?? string.Empty;
        }

        public string ToDisplayString()
        {
            return $"[{Date}] {Prompt}\n{Response}";
        }

        public string ToStorageString(string delimiter)
        {
            return string.Join(delimiter, new[] { Date, Prompt, Response });
        }

        public static Entry FromStorageString(string storedValue, string delimiter)
        {
            if (string.IsNullOrWhiteSpace(storedValue))
            {
                throw new ArgumentException("Stored value cannot be null or empty.", nameof(storedValue));
            }

            var parts = storedValue.Split(new[] { delimiter }, StringSplitOptions.None);
            if (parts.Length != 3)
            {
                throw new FormatException("Stored entry does not have the expected number of parts.");
            }

            return new Entry(parts[1], parts[2], parts[0]);
        }
    }
}
