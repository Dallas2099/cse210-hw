using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Extra credit: Added a level progression system plus persistent achievements/badges with milestone alerts.

class Program
{
    static void Main(string[] args)
    {
        var manager = new GoalManager();

        Console.WriteLine("Welcome to the Eternal Quest tracker!");

        bool exitRequested = false;
        while (!exitRequested)
        {
            Console.WriteLine();
            Console.WriteLine($"Total Score: {manager.Score} pts  |  Level: {manager.Level}");
            Console.WriteLine("Menu Options:");
            Console.WriteLine(" 1. List goals");
            Console.WriteLine(" 2. Create new goal");
            Console.WriteLine(" 3. Record an event");
            Console.WriteLine(" 4. Save goals");
            Console.WriteLine(" 5. Load goals");
            Console.WriteLine(" 6. Show achievements");
            Console.WriteLine(" 7. Quit");
            Console.Write("Select a choice from the menu: ");

            string choice = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    DisplayGoals(manager);
                    break;
                case "2":
                    CreateGoalFlow(manager);
                    break;
                case "3":
                    RecordEventFlow(manager);
                    break;
                case "4":
                    SaveGoalsFlow(manager);
                    break;
                case "5":
                    LoadGoalsFlow(manager);
                    break;
                case "6":
                    DisplayAchievements(manager);
                    break;
                case "7":
                    exitRequested = true;
                    break;
                default:
                    Console.WriteLine("Please choose a valid option (1-7).");
                    break;
            }
        }

        Console.WriteLine("Thanks for working on your Eternal Quest today!");
    }

    private static void DisplayGoals(GoalManager manager)
    {
        if (!manager.Goals.Any())
        {
            Console.WriteLine("No goals have been created yet.");
            return;
        }

        Console.WriteLine("Your goals:");
        for (int i = 0; i < manager.Goals.Count; i++)
        {
            Console.WriteLine($" {i + 1}. {manager.Goals[i].GetDisplayText()}");
        }
    }

    private static void DisplayAchievements(GoalManager manager)
    {
        if (!manager.Achievements.Any())
        {
            Console.WriteLine("You haven't unlocked any achievements yet—keep going!");
            return;
        }

        Console.WriteLine("Unlocked achievements:");
        foreach (var achievement in manager.GetUnlockedAchievementDetails())
        {
            Console.WriteLine($" - {achievement.Name}: {achievement.Description}");
        }
    }

    private static void CreateGoalFlow(GoalManager manager)
    {
        Console.WriteLine("The types of goals are:");
        Console.WriteLine(" 1. Simple Goal");
        Console.WriteLine(" 2. Eternal Goal");
        Console.WriteLine(" 3. Checklist Goal");
        Console.Write("Which type of goal would you like to create? ");

        string selection = Console.ReadLine() ?? string.Empty;
        Console.WriteLine();

        Goal goal;
        switch (selection)
        {
            case "1":
                goal = CreateSimpleGoal();
                break;
            case "2":
                goal = CreateEternalGoal();
                break;
            case "3":
                goal = CreateChecklistGoal();
                break;
            default:
                Console.WriteLine("Goal creation cancelled.");
                return;
        }

        var newAchievements = manager.AddGoal(goal);
        Console.WriteLine($"Goal \"{goal.Name}\" added.");
        AnnounceAchievements(newAchievements);
    }

    private static void RecordEventFlow(GoalManager manager)
    {
        if (!manager.Goals.Any())
        {
            Console.WriteLine("Create a goal before recording events.");
            return;
        }

        Console.WriteLine("Record progress for which goal?");
        for (int i = 0; i < manager.Goals.Count; i++)
        {
            Console.WriteLine($" {i + 1}. {manager.Goals[i].Name}");
        }

        int selection = PromptForInt("Enter goal number: ", 1, manager.Goals.Count);
        var result = manager.RecordEvent(selection - 1);

        Console.WriteLine(result.Message);
        AnnounceAchievements(result.NewAchievements);
    }

    private static void SaveGoalsFlow(GoalManager manager)
    {
        Console.Write("Enter the filename to save to (e.g., goals.txt): ");
        string fileNameInput = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(fileNameInput))
        {
            Console.WriteLine("Save cancelled—no filename provided.");
            return;
        }

        try
        {
            manager.SaveToFile(fileNameInput.Trim());
            Console.WriteLine("Goals saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to save goals: {ex.Message}");
        }
    }

    private static void LoadGoalsFlow(GoalManager manager)
    {
        Console.Write("Enter the filename to load from: ");
        string fileNameInput = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(fileNameInput))
        {
            Console.WriteLine("Load cancelled—no filename provided.");
            return;
        }

        try
        {
            manager.LoadFromFile(fileNameInput.Trim());
            Console.WriteLine("Goals loaded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to load goals: {ex.Message}");
        }
    }

    private static void AnnounceAchievements(IEnumerable<string> achievements)
    {
        if (achievements is null)
        {
            return;
        }

        var names = achievements.ToList();
        if (!names.Any())
        {
            return;
        }

        Console.WriteLine("New achievements unlocked!");
        foreach (string name in names)
        {
            Console.WriteLine($" * {name}");
        }
    }

    private static Goal CreateSimpleGoal()
    {
        string name = PromptForText("What is the name of your goal? ");
        string description = PromptForText("What is a short description of it? ");
        int points = PromptForInt("What is the amount of points associated with this goal? ", 0);
        return new SimpleGoal(name, description, points);
    }

    private static Goal CreateEternalGoal()
    {
        string name = PromptForText("What is the name of your eternal goal? ");
        string description = PromptForText("What is a short description of it? ");
        int points = PromptForInt("How many points are earned each time you record it? ", 0);
        return new EternalGoal(name, description, points);
    }

    private static Goal CreateChecklistGoal()
    {
        string name = PromptForText("What is the name of your checklist goal? ");
        string description = PromptForText("What is a short description of it? ");
        int points = PromptForInt("How many points are earned each time you record it? ", 0);
        int targetCount = PromptForInt("How many times does this goal need to be accomplished for a bonus? ", 1);
        int bonus = PromptForInt("What is the bonus for completing it that many times? ", 0);
        return new ChecklistGoal(name, description, points, targetCount, bonus);
    }

    private static string PromptForText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string value = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Please enter a value.");
        }
    }

    private static int PromptForInt(string prompt, int minValue, int? maxValue = null)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(input, out int result))
            {
                if (result < minValue)
                {
                    Console.WriteLine($"Enter a value greater than or equal to {minValue}.");
                    continue;
                }

                if (maxValue.HasValue && result > maxValue.Value)
                {
                    Console.WriteLine($"Enter a value no greater than {maxValue.Value}.");
                    continue;
                }

                return result;
            }

            Console.WriteLine("Please enter a whole number.");
        }
    }
}
