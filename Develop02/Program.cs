using System;
using System.Collections.Generic;
using Develop02;

class Program
{
    private const string StorageDelimiter = "|~|";

    static void Main(string[] args)
    {
        var prompts = new List<string>
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What is one small win I can celebrate from today?",
            "What is something I learned or rediscovered today?"
        };

        var promptGenerator = new PromptGenerator(prompts);
        var journal = new Journal(StorageDelimiter);

        var running = true;
        while (running)
        {
            ShowMenu();
            Console.Write("Select an option: ");
            var choice = (Console.ReadLine() ?? string.Empty).Trim();

            switch (choice)
            {
                case "1":
                    WriteNewEntry(journal, promptGenerator);
                    break;
                case "2":
                    journal.Display();
                    break;
                case "3":
                    SaveJournal(journal);
                    break;
                case "4":
                    LoadJournal(journal);
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please choose a valid option (1-5).\n");
                    break;
            }
        }

        Console.WriteLine("Goodbye!");
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Journal Menu");
        Console.WriteLine("1. Write a new entry");
        Console.WriteLine("2. Display the journal");
        Console.WriteLine("3. Save the journal to a file");
        Console.WriteLine("4. Load the journal from a file");
        Console.WriteLine("5. Quit");
    }

    private static void WriteNewEntry(Journal journal, PromptGenerator promptGenerator)
    {
        var prompt = promptGenerator.GetRandomPrompt();
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.Write("Your response: ");
        var response = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(response))
        {
            Console.WriteLine("No response recorded. Entry skipped.\n");
            return;
        }

        var date = DateTime.Now.ToString("yyyy-MM-dd");
        var entry = new Entry(prompt, response.Trim(), date);
        journal.AddEntry(entry);
        Console.WriteLine("Entry added.\n");
    }

    private static void SaveJournal(Journal journal)
    {
        Console.Write("Enter the file name to save to: ");
        var fileName = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("A file name is required to save the journal.\n");
            return;
        }

        try
        {
            journal.SaveToFile(fileName.Trim());
            Console.WriteLine("Journal saved successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to save the journal: {ex.Message}\n");
        }
    }

    private static void LoadJournal(Journal journal)
    {
        Console.Write("Enter the file name to load from: ");
        var fileName = Console.ReadLine() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(fileName))
        {
            Console.WriteLine("A file name is required to load the journal.\n");
            return;
        }

        try
        {
            journal.LoadFromFile(fileName.Trim());
            Console.WriteLine("Journal loaded successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load the journal: {ex.Message}\n");
        }
    }
}
