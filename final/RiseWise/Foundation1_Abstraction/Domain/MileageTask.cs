namespace Foundation1_Abstraction.Domain;

/// <summary>
/// A maintenance task that becomes due based on elapsed miles.
/// </summary>
public sealed class MileageTask : MaintenanceTask
{
    public MileageTask(string id, string name, int intervalMiles, DateOnly? lastDoneDate, int? lastDoneOdometer)
        : base(id, name, lastDoneDate, lastDoneOdometer)
    {
        if (intervalMiles <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalMiles), "Interval miles must be positive.");
        }

        IntervalMiles = intervalMiles;
    }

    public int IntervalMiles { get; }

    public override bool IsDue(DateOnly today, int currentOdometer)
    {
        if (LastDoneOdometer is null)
        {
            return currentOdometer >= IntervalMiles;
        }

        return currentOdometer - LastDoneOdometer.Value >= IntervalMiles;
    }

    public override string NextDueHint()
    {
        var baseline = LastDoneOdometer ?? 0;
        var nextDueOdometer = baseline + IntervalMiles;
        return $"Next due at {nextDueOdometer:N0} miles";
    }
}
