namespace RiseWiseMoto.Domain;

public sealed class ReadingTask : MaintenanceTask
{
    public ReadingTask(string name, double threshold) : base(name)
    {
        Threshold = threshold;
    }

    public ReadingTask()
    {
        Threshold = 3.0;
    }

    public double Threshold { get; set; }

    public double? LastReading { get; set; }

    public override bool IsDue(DateOnly today, int currentOdometer)
    {
        if (LastReading is null)
        {
            return true;
        }

        return LastReading <= Threshold;
    }

    public override bool IsDueSoon(DateOnly today, int currentOdometer)
    {
        if (LastReading is null)
        {
            return false;
        }

        return LastReading <= Threshold + 0.5;
    }

    public override string NextDueHint() => "TODO: Calibrate reading-based reminders.";
}
