namespace RiseWiseMoto.Domain;

public sealed class MileageTask : MaintenanceTask
{
    public MileageTask(string name, int intervalMiles) : base(name)
    {
        if (intervalMiles <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intervalMiles), "Interval must be positive.");
        }

        IntervalMiles = intervalMiles;
    }

    public MileageTask()
    {
        IntervalMiles = 1_000;
    }

    public int IntervalMiles { get; set; }

    public override bool IsDue(DateOnly today, int currentOdometer)
    {
        if (LastOdometer is null)
        {
            return currentOdometer >= IntervalMiles;
        }

        return currentOdometer - LastOdometer.Value >= IntervalMiles;
    }

    public override bool IsDueSoon(DateOnly today, int currentOdometer)
    {
        if (IsDue(today, currentOdometer))
        {
            return false;
        }

        var baseline = LastOdometer ?? 0;
        var milesRemaining = IntervalMiles - (currentOdometer - baseline);
        return milesRemaining <= IntervalMiles * 0.25; // Warn when within the final quarter of the interval.
    }

    public override string NextDueHint()
    {
        var baseline = LastOdometer ?? 0;
        var nextDueOdometer = baseline + IntervalMiles;
        return $"Next due at {nextDueOdometer:N0} miles";
    }
}
