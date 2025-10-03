using System;
using System.Collections.Generic;
using System.Linq;

class Scripture
{
    private static readonly Random _random = new Random();

    private readonly Reference _reference;
    private readonly List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference ?? throw new ArgumentNullException(nameof(reference));

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Scripture text cannot be empty.", nameof(text));
        }

        _words = text
            .Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(part => new Word(part))
            .ToList();
    }

    public Reference Reference => _reference;

    public int TotalWordCount => _words.Count;

    public bool AllWordsHidden => _words.All(word => word.IsHidden);

    public int GetHiddenWordCount()
    {
        return _words.Count(word => word.IsHidden);
    }

    public string GetDisplayText()
    {
        string rendered = string.Join(" ", _words.Select(word => word.GetDisplayText()));
        return $"{_reference} {rendered}";
    }

    public void HideRandomWords(int wordCount)
    {
        if (wordCount <= 0)
        {
            return;
        }

        List<Word> available = _words.Where(word => !word.IsHidden).ToList();
        if (available.Count == 0)
        {
            return;
        }

        int toHide = Math.Min(wordCount, available.Count);
        for (int i = 0; i < toHide; i++)
        {
            int index = _random.Next(available.Count);
            available[index].Hide();
            available.RemoveAt(index);
        }
    }

    public bool RevealHint()
    {
        List<Word> hiddenWords = _words.Where(word => word.IsHidden).ToList();
        if (hiddenWords.Count == 0)
        {
            return false;
        }

        int index = _random.Next(hiddenWords.Count);
        hiddenWords[index].Reveal();
        return true;
    }
}
