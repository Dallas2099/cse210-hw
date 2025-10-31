namespace Foundation1_Abstraction.Domain;

/// <summary>
/// A maintenance task that becomes due after a certain number of days.
/// </summary>
public sealed class TimeTask : MaintenanceTask
{
    public TimeTask(string id, string name, int intervalDays, DateOnly? lastDoneDate, int? lastDoneOdometer)
        : base(id, name, lastDoneDate, lastDoneOdometer)
    {
        if (intervalDays <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalDays), "Interval days must be positive.");
        }

        IntervalDays = intervalDays;
    }

    public int IntervalDays { get; }

    public override bool IsDue(DateOnly today, int currentOdometer)
    {
        if (LastDoneDate is null)
        {
            return true;
        }

        return today.DayNumber - LastDoneDate.Value.DayNumber >= IntervalDays;
    }

    public override string NextDueHint()
    {
        if (LastDoneDate is null)
        {
            return $"Due every {IntervalDays} days";
        }

        var nextDate = LastDoneDate.Value.AddDays(IntervalDays);
        return $"Next due on {nextDate:MMM d, yyyy}";
    }
}
