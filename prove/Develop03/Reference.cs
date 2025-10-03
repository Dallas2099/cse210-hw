using System;

class Reference
{
    public string Book { get; }
    public int Chapter { get; }
    public int StartVerse { get; }
    public int? EndVerse { get; }

    public Reference(string book, int chapter, int verse)
        : this(book, chapter, verse, null)
    {
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
        : this(book, chapter, startVerse, endVerse == startVerse ? (int?)null : endVerse)
    {
    }

    private Reference(string book, int chapter, int startVerse, int? endVerse)
    {
        if (string.IsNullOrWhiteSpace(book))
        {
            throw new ArgumentException("Book cannot be empty.", nameof(book));
        }

        if (chapter <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chapter), "Chapter must be positive.");
        }

        if (startVerse <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startVerse), "Start verse must be positive.");
        }

        if (endVerse.HasValue && endVerse.Value < startVerse)
        {
            throw new ArgumentException("End verse must be greater than or equal to the start verse.", nameof(endVerse));
        }

        Book = book;
        Chapter = chapter;
        StartVerse = startVerse;
        EndVerse = endVerse;
    }

    public override string ToString()
    {
        return EndVerse.HasValue
            ? $"{Book} {Chapter}:{StartVerse}-{EndVerse}"
            : $"{Book} {Chapter}:{StartVerse}";
    }
}
