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
