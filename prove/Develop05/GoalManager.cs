using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            message += $" You advanced to level {Level}!";
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
