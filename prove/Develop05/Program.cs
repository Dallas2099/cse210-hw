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

abstract class Goal
{
    protected Goal(string name, string description, int points)
    {
        Name = name;
        Description = description;
        Points = points;
    }

    public string Name { get; }
    public string Description { get; }
    public int Points { get; }

    public abstract bool IsComplete { get; }

    protected string CompletionBox => IsComplete ? "[X]" : "[ ]";

    public abstract int RecordEvent();

    public virtual string GetDisplayText()
    {
        return $"{CompletionBox} {Name} ({Description})";
    }

    public abstract string Serialize();
}

class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points, bool isComplete = false)
        : base(name, description, points)
    {
        _isComplete = isComplete;
    }

    public override bool IsComplete => _isComplete;

    public override int RecordEvent()
    {
        if (_isComplete)
        {
            return 0;
        }

        _isComplete = true;
        return Points;
    }

    public override string Serialize()
    {
        return $"SimpleGoal|{Name}|{Description}|{Points}|{_isComplete}";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
    }

    public override bool IsComplete => false;

    public override int RecordEvent()
    {
        return Points;
    }

    public override string Serialize()
    {
        return $"EternalGoal|{Name}|{Description}|{Points}";
    }
}

class ChecklistGoal : Goal
{
    private int _currentCount;

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints, int currentCount = 0)
        : base(name, description, points)
    {
        TargetCount = targetCount;
        BonusPoints = bonusPoints;
        _currentCount = currentCount;
    }

    public int TargetCount { get; }
    public int BonusPoints { get; }

    public override bool IsComplete => _currentCount >= TargetCount;

    public override int RecordEvent()
    {
        if (IsComplete)
        {
            return 0;
        }

        _currentCount++;
        int total = Points;
        if (IsComplete)
        {
            total += BonusPoints;
        }

        return total;
    }

    public override string GetDisplayText()
    {
        return $"{CompletionBox} {Name} ({Description}) -- Completed {_currentCount}/{TargetCount}";
    }

    public override string Serialize()
    {
        return $"ChecklistGoal|{Name}|{Description}|{Points}|{_currentCount}|{TargetCount}|{BonusPoints}";
    }
}

static class GoalFactory
{
    public static Goal Deserialize(string dataLine)
    {
        if (string.IsNullOrWhiteSpace(dataLine))
        {
            throw new InvalidDataException("Goal data was empty.");
        }

        string[] parts = dataLine.Split('|', StringSplitOptions.None);
        if (parts.Length == 0)
        {
            throw new InvalidDataException("Goal data was missing a type identifier.");
        }

        return parts[0] switch
        {
            "SimpleGoal" when parts.Length >= 5
                => new SimpleGoal(parts[1], parts[2], ParseInt(parts[3]), bool.Parse(parts[4])),
            "EternalGoal" when parts.Length >= 4
                => new EternalGoal(parts[1], parts[2], ParseInt(parts[3])),
            "ChecklistGoal" when parts.Length >= 7
                => new ChecklistGoal(
                    parts[1],
                    parts[2],
                    ParseInt(parts[3]),
                    ParseInt(parts[5]),
                    ParseInt(parts[6]),
                    ParseInt(parts[4])),
            _ => throw new InvalidDataException($"Unrecognised goal entry: {dataLine}")
        };
    }

    private static int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }

        throw new InvalidDataException($"Invalid numeric value '{value}' in goal data.");
    }
}

class RecordEventResult
{
    private RecordEventResult(bool isSuccessful, int pointsEarned, bool levelUp, List<string> newAchievements, string message)
    {
        IsSuccessful = isSuccessful;
        PointsEarned = pointsEarned;
        LevelUp = levelUp;
        NewAchievements = newAchievements.AsReadOnly();
        Message = message;
    }

    public bool IsSuccessful { get; }
    public int PointsEarned { get; }
    public bool LevelUp { get; }
    public IReadOnlyList<string> NewAchievements { get; }
    public string Message { get; }

    public static RecordEventResult Failure(string message)
    {
        return new RecordEventResult(false, 0, false, new List<string>(), message);
    }

    public static RecordEventResult Success(int pointsEarned, bool levelUp, List<string> newAchievements, string message)
    {
        return new RecordEventResult(true, pointsEarned, levelUp, newAchievements, message);
    }
}

class GoalManager
{
    private readonly List<Goal> _goals = new();
    private readonly HashSet<string> _earnedBadges = new();
    private readonly Dictionary<string, string> _badgeDescriptions = new()
    {
        ["First Steps"] = "Earn at least 100 total points.",
        ["Bronze Adventurer"] = "Earn at least 500 total points.",
        ["Silver Pathfinder"] = "Earn at least 1000 total points.",
        ["Gold Trailblazer"] = "Earn at least 2000 total points.",
        ["Checklist Conqueror"] = "Complete any checklist goal.",
        ["Simple Specialist"] = "Complete three simple goals.",
        ["Goal Collector"] = "Create five or more goals."
    };

    public IReadOnlyList<Goal> Goals => _goals;
    public IReadOnlyCollection<string> Achievements => _earnedBadges;
    public int Score { get; private set; }
    public int Level => Math.Max(1, (Score / 500) + 1);

    public List<string> AddGoal(Goal goal)
    {
        _goals.Add(goal);
        return EvaluateAchievements();
    }

    public RecordEventResult RecordEvent(int goalIndex)
    {
        if (goalIndex < 0 || goalIndex >= _goals.Count)
        {
            return RecordEventResult.Failure("That goal number is not valid.");
        }

        Goal goal = _goals[goalIndex];

        int previousLevel = Level;
        int pointsEarned = goal.RecordEvent();

        if (pointsEarned <= 0)
        {
            string note = goal.IsComplete
                ? $"The goal \"{goal.Name}\" is already complete."
                : "No points were earned for that entry.";
            return RecordEventResult.Failure(note);
        }

        Score += pointsEarned;

        List<string> newAchievements = EvaluateAchievements();
        bool leveledUp = Level > previousLevel;

        string message = $"Fantastic! You earned {pointsEarned} points.";
        if (leveledUp)
        {
            message += $" You advanced to Level {Level}!";
        }

        return RecordEventResult.Success(pointsEarned, leveledUp, newAchievements, message);
    }

    public void SaveToFile(string filePath)
    {
        var lines = new List<string>
        {
            $"Score|{Score}",
            $"Badges|{string.Join(';', _earnedBadges)}"
        };

        foreach (Goal goal in _goals)
        {
            lines.Add(goal.Serialize());
        }

        File.WriteAllLines(filePath, lines);
    }

    public void LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Could not find the requested save file.", filePath);
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
        {
            throw new InvalidDataException("The save file is empty.");
        }

        _goals.Clear();
        _earnedBadges.Clear();

        Score = ParseScore(lines[0]);

        int index = 1;
        if (lines.Length > 1 && lines[1].StartsWith("Badges|", StringComparison.OrdinalIgnoreCase))
        {
            string badgeData = lines[1].Substring("Badges|".Length);
            if (!string.IsNullOrWhiteSpace(badgeData))
            {
                foreach (string badge in badgeData.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    _earnedBadges.Add(badge);
                }
            }

            index = 2;
        }

        for (int i = index; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            _goals.Add(GoalFactory.Deserialize(line));
        }

        EvaluateAchievements(); // ensure score-based badges are re-awarded if necessary
    }

    public IEnumerable<(string Name, string Description)> GetUnlockedAchievementDetails()
    {
        return _earnedBadges
            .OrderBy(b => b)
            .Select(b => (b, _badgeDescriptions.TryGetValue(b, out string description) ? description : "Achievement unlocked."));
    }

    private List<string> EvaluateAchievements()
    {
        var unlocked = new List<string>();

        void AddBadge(string name)
        {
            if (_earnedBadges.Add(name))
            {
                unlocked.Add(name);
            }
        }

        if (Score >= 100)
        {
            AddBadge("First Steps");
        }

        if (Score >= 500)
        {
            AddBadge("Bronze Adventurer");
        }

        if (Score >= 1000)
        {
            AddBadge("Silver Pathfinder");
        }

        if (Score >= 2000)
        {
            AddBadge("Gold Trailblazer");
        }

        if (_goals.OfType<SimpleGoal>().Count(goal => goal.IsComplete) >= 3)
        {
            AddBadge("Simple Specialist");
        }

        if (_goals.OfType<ChecklistGoal>().Any(goal => goal.IsComplete))
        {
            AddBadge("Checklist Conqueror");
        }

        if (_goals.Count >= 5)
        {
            AddBadge("Goal Collector");
        }

        return unlocked;
    }

    private static int ParseScore(string scoreLine)
    {
        if (scoreLine.StartsWith("Score|", StringComparison.OrdinalIgnoreCase))
        {
            string value = scoreLine.Substring("Score|".Length);
            if (int.TryParse(value, out int score) && score >= 0)
            {
                return score;
            }
        }

        throw new InvalidDataException("The save file contained an invalid score value.");
    }
}
