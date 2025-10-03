using System;
using System.Collections.Generic;

class Program
{
    // Exceeded requirements: scripture library with optional random selection, adjustable difficulty,
    // and a hint command that reveals a hidden word for targeted practice.
    private const int DefaultWordsPerRound = 3;
    private static readonly Random _random = new Random();

    static void Main(string[] args)
    {
        List<Scripture> library = BuildLibrary();
        Scripture scripture = SelectScripture(library);
        int wordsPerRound = PromptForWordsPerRound();

        RunMemorizer(scripture, wordsPerRound);
    }

    private static List<Scripture> BuildLibrary()
    {
        return new List<Scripture>
        {
            new Scripture(
                new Reference("John", 3, 16),
                "For God so loved the world, that he gave his only begotten Son, that whosoever believeth in him " +
                "should not perish, but have everlasting life."),
            new Scripture(
                new Reference("Proverbs", 3, 5, 6),
                "Trust in the Lord with all thine heart; and lean not unto thine own understanding. In all thy ways " +
                "acknowledge him, and he shall direct thy paths."),
            new Scripture(
                new Reference("Mosiah", 2, 41),
                "Consider on the blessed and happy state of those that keep the commandments of God. For behold, they " +
                "are blessed in all things, both temporal and spiritual; and if they hold out faithful to the end they " +
                "are received into heaven, that thereby they may dwell with God in a state of never-ending happiness.")
        };
    }

    private static Scripture SelectScripture(List<Scripture> scriptures)
    {
        Console.Clear();
        Console.WriteLine("Scripture Memorizer");
        Console.WriteLine("-------------------");
        for (int i = 0; i < scriptures.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {scriptures[i].Reference}");
        }

        Console.WriteLine();
        Console.Write("Enter the number of a scripture or press Enter for a random one: ");
        string input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input) &&
            int.TryParse(input, out int chosenIndex) &&
            chosenIndex >= 1 && chosenIndex <= scriptures.Count)
        {
            return scriptures[chosenIndex - 1];
        }

        return scriptures[_random.Next(scriptures.Count)];
    }

    private static int PromptForWordsPerRound()
    {
        Console.WriteLine();
        Console.Write($"How many words should be hidden each round? (press Enter for {DefaultWordsPerRound}): ");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return DefaultWordsPerRound;
        }

        if (int.TryParse(input.Trim(), out int wordsPerRound) && wordsPerRound > 0)
        {
            return wordsPerRound;
        }

        Console.WriteLine("Invalid entry detected. Using the default pacing.");
        Console.WriteLine("Press Enter to begin.");
        Console.ReadLine();
        return DefaultWordsPerRound;
    }

    private static void RunMemorizer(Scripture scripture, int wordsPerRound)
    {
        string statusMessage = string.Empty;

        while (true)
        {
            Console.Clear();

            Console.WriteLine(scripture.GetDisplayText());
            Console.WriteLine();
            Console.WriteLine($"Hidden words: {scripture.GetHiddenWordCount()} / {scripture.TotalWordCount}");

            if (!string.IsNullOrEmpty(statusMessage))
            {
                Console.WriteLine();
                Console.WriteLine(statusMessage);
                statusMessage = string.Empty;
            }

            if (scripture.AllWordsHidden)
            {
                Console.WriteLine();
                Console.WriteLine("All words are hidden. Great work!");
                break;
            }

            Console.WriteLine();
            Console.Write("Press Enter to hide more words, type 'hint' for a clue, or 'quit' to exit: ");
            string input = Console.ReadLine();
            if (input is null)
            {
                continue;
            }

            string command = input.Trim();

            if (command.Equals("quit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            if (command.Length == 0)
            {
                int before = scripture.GetHiddenWordCount();
                scripture.HideRandomWords(wordsPerRound);
                int after = scripture.GetHiddenWordCount();
                int hiddenThisRound = after - before;

                statusMessage = hiddenThisRound > 0
                    ? $"Hid {hiddenThisRound} word(s). Keep going!"
                    : "All words were already hidden.";
                continue;
            }

            if (command.Equals("hint", StringComparison.OrdinalIgnoreCase))
            {
                statusMessage = scripture.RevealHint()
                    ? "Hint: revealed one word to jog your memory."
                    : "No hint available because nothing is hidden yet.";
                continue;
            }

            statusMessage = "Command not recognized. Please press Enter, type 'hint', or 'quit'.";
        }

        Console.WriteLine();
        Console.WriteLine("Thanks for practicing today!");
    }
}
