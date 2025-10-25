using System;

class Word
{
    private readonly string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public bool IsHidden => _isHidden;

    public void Hide()
    {
        _isHidden = true;
    }

    public void Reveal()
    {
        _isHidden = false;
    }

    public string GetDisplayText()
    {
        if (!_isHidden)
        {
            return _text;
        }

        char[] masked = new char[_text.Length];
        for (int i = 0; i < _text.Length; i++)
        {
            char character = _text[i];
            masked[i] = char.IsLetterOrDigit(character) ? '_' : character;
        }

        return new string(masked);
    }
}
