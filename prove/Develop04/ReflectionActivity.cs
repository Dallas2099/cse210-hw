using System;
using System.Collections.Generic;

public class ReflectionActivity : MindfulnessActivity
{
    private static readonly List<string> _prompts = new()
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private static readonly List<string> _questions = new()
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    private readonly Queue<string> _promptQueue = new();
    private readonly Queue<string> _questionQueue = new();

    public ReflectionActivity()
        : base(
            "Reflection Activity",
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
    }

    protected override void PerformActivity()
    {
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine();
        Console.WriteLine($"--- {GetNextPrompt()} ---");
        Console.WriteLine();
        Console.Write("When you have something in mind, press Enter to continue. ");
        Console.ReadLine();

        Console.WriteLine();
        Console.WriteLine("Now ponder on each of the following questions as they relate to this experience.");
        Console.Write("You may begin in: ");
        ShowCountdown(5);
        Console.Clear();

        Console.WriteLine("Reflect on the following questions:");
        DateTime endTime = DateTime.Now.AddSeconds(Duration);

        while (DateTime.Now < endTime)
        {
            string question = GetNextQuestion();
            Console.Write($"> {question} ");
            ShowSpinner(6);
            Console.WriteLine();
        }

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

    private string GetNextQuestion()
    {
        if (_questionQueue.Count == 0)
        {
            EnqueueRandomOrder(_questionQueue, _questions);
        }

        return _questionQueue.Dequeue();
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
