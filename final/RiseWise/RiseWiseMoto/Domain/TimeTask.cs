namespace RiseWiseMoto.Domain;

public sealed class TimeTask : MaintenanceTask
{
    public TimeTask(string name, int intervalDays) : base(name)
    {
        if (intervalDays <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalDays), "Interval must be positive.");
        }

        IntervalDays = intervalDays;
    }

    public TimeTask()
    {
        IntervalDays = 30;
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
        return (nextDate.DayNumber - today.DayNumber) <= 7;
    }

    public override string NextDueHint()
    {
        if (LastCompletedOn is null)
        {
            return $"Due every {IntervalDays} days";
        }

        var nextDate = LastCompletedOn.Value.AddDays(IntervalDays);
        return $"Next due on {nextDate:MMM d, yyyy}";
    }
}
