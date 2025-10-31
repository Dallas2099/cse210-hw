namespace RiseWiseMoto.Domain;

public abstract class MaintenanceTask
{
    protected MaintenanceTask(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Task name is required.", nameof(name));
        }

        Name = name;
    }

    protected MaintenanceTask()
    {
        Name = string.Empty;
    }

    public string Name { get; set; }

    public DateOnly? LastCompletedOn { get; set; }

    public int? LastOdometer { get; set; }

    public void RecordCompletion(DateOnly completedOn, int odometerReading)
    {
        LastCompletedOn = completedOn;
        LastOdometer = odometerReading;
    }

    public abstract bool IsDue(DateOnly today, int currentOdometer);

    public abstract string NextDueHint();

    public virtual bool IsDueSoon(DateOnly today, int currentOdometer) => false;
}
