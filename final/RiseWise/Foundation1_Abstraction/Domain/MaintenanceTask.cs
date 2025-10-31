namespace Foundation1_Abstraction.Domain;

/// <summary>
/// Base abstraction for any maintenance task tracked by the Moto app.
/// </summary>
public abstract class MaintenanceTask
{
    protected MaintenanceTask(string id, string name, DateOnly? lastDoneDate, int? lastDoneOdometer)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Task id is required", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Task name is required", nameof(name));
        }

        Id = id;
        Name = name;
        LastDoneDate = lastDoneDate;
        LastDoneOdometer = lastDoneOdometer;
    }

    public string Id { get; }
    public string Name { get; }

    public DateOnly? LastDoneDate { get; private set; }
    public int? LastDoneOdometer { get; private set; }

    public void MarkCompleted(DateOnly dateCompleted, int odometerReading)
    {
        LastDoneDate = dateCompleted;
        LastDoneOdometer = odometerReading;
    }

    public abstract bool IsDue(DateOnly today, int currentOdometer);

    public abstract string NextDueHint();
}
