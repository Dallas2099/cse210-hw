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
