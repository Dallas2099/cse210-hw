using System;
using System.Threading;

public abstract class MindfulnessActivity
{
    private readonly string _name;
    private readonly string _description;
    private int _duration;
    private static readonly Random _random = new Random();

    protected MindfulnessActivity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    protected int Duration => _duration;

    protected static Random Random => _random;

    public void Run()
    {
        DisplayStartingMessage();
        PerformActivity();
        DisplayEndingMessage();
    }

    protected abstract void PerformActivity();

    private void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine();
        Console.WriteLine(_description);
        Console.WriteLine();
        _duration = PromptForDuration();

        Console.Clear();
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
        Console.WriteLine();
    }

    private int PromptForDuration()
    {
        while (true)
        {
            Console.Write("How long, in seconds, would you like for your session? ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int seconds) && seconds > 0)
            {
                return seconds;
            }

            Console.WriteLine("Please enter a positive whole number.");
        }
    }

    private void DisplayEndingMessage()
    {
        Console.WriteLine();
        Console.WriteLine("Great job!");
        ShowSpinner(2);
        Console.WriteLine($"You have completed the {_name} for {_duration} seconds.");
        ShowSpinner(3);
        Console.WriteLine();
    }

    protected void ShowSpinner(int seconds)
    {
        string[] sequence = { "|", "/", "-", "\\" };
        DateTime endTime = DateTime.Now.AddSeconds(seconds);
        int index = 0;

        while (DateTime.Now < endTime)
        {
            Console.Write(sequence[index]);
            Thread.Sleep(250);
            Console.Write("\b");
            index = (index + 1) % sequence.Length;
        }

        Console.Write(" ");
        Console.Write("\b");
    }

    protected void ShowCountdown(int seconds)
    {
        for (int remaining = seconds; remaining > 0; remaining--)
        {
            string text = remaining.ToString();
            Console.Write(text);
            Thread.Sleep(1000);
            Console.Write(new string('\b', text.Length));
            Console.Write(new string(' ', text.Length));
            Console.Write(new string('\b', text.Length));
        }

        Console.WriteLine();
    }
}
