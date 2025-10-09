using System;
using System.Collections.Generic;

public class ListingActivity : MindfulnessActivity
{
    private static readonly List<string> _prompts = new()
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    private readonly Queue<string> _promptQueue = new();

    public ListingActivity()
        : base(
            "Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    protected override void PerformActivity()
    {
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine();
        Console.WriteLine($"--- {GetNextPrompt()} ---");
        Console.WriteLine();
        Console.Write("You may begin in: ");
        ShowCountdown(5);
        Console.WriteLine();

        DateTime endTime = DateTime.Now.AddSeconds(Duration);
        int itemsListed = 0;

        // We allow the user to finish their current thought even if the time expires mid-response.
        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string? response = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(response))
            {
                itemsListed++;
            }
        }

        Console.WriteLine();
        Console.WriteLine($"You listed {itemsListed} items!");
        Console.WriteLine();
    }

    private string GetNextPrompt()
    {
        if (_promptQueue.Count == 0)
        {
            EnqueueRandomOrder(_promptQueue, _prompts);
        }

        return _promptQueue.Dequeue();
    }

    private void EnqueueRandomOrder(Queue<string> queue, List<string> source)
    {
        List<string> shuffled = new(source);
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int swapIndex = Random.Next(i + 1);
            (shuffled[i], shuffled[swapIndex]) = (shuffled[swapIndex], shuffled[i]);
        }

        foreach (string item in shuffled)
        {
            queue.Enqueue(item);
        }
    }
}
