namespace RiseWiseMoto.Domain;

public sealed class ChecklistTask : MaintenanceTask
{
    public ChecklistTask(string name, int intervalDays) : base(name)
    {
        IntervalDays = intervalDays;
    }

    public ChecklistTask()
    {
        IntervalDays = 7;
    }

    public int IntervalDays { get; set; }

    public override bool IsDue(DateOnly today, int currentOdometer)
    {
        if (LastCompletedOn is null)
        {
            return true;
        }

        return today.DayNumber - LastCompletedOn.Value.DayNumber >= IntervalDays;
    }

    public override bool IsDueSoon(DateOnly today, int currentOdometer)
    {
        if (IsDue(today, currentOdometer))
        {
            return false;
        }

        if (LastCompletedOn is null)
        {
            return true;
        }

        var nextDate = LastCompletedOn.Value.AddDays(IntervalDays);
        return (nextDate.DayNumber - today.DayNumber) <= 2;
    }

    public override string NextDueHint() => "TODO: Add custom checklist instructions.";
}
