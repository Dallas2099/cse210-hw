namespace RiseWiseMoto.Domain;

public class Motorcycle
{
    private int _odometer;

    public Motorcycle(string make, string model, int year, string vin, int initialOdometer)
    {
        if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
        {
            throw new ArgumentException("Make and model are required to describe a motorcycle.");
        }

        if (string.IsNullOrWhiteSpace(vin))
        {
            throw new ArgumentException("VIN is required.", nameof(vin));
        }

        if (year < 1970 || year > DateTime.UtcNow.Year + 1)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Year looks unrealistic for this prototype.");
        }

        Make = make;
        Model = model;
        Year = year;
        Vin = vin;

        UpdateOdometer(initialOdometer);
    }

    public Motorcycle()
    {
        Make = string.Empty;
        Model = string.Empty;
        Vin = string.Empty;
    }

    public string Make { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }
    public string Vin { get; init; } = string.Empty;

    public int Odometer
    {
        get => _odometer;
        private set => _odometer = value;
    }

    public List<MaintenanceTask> Tasks { get; } = new();

    public void UpdateOdometer(int miles)
    {
        if (miles < _odometer)
        {
            throw new InvalidOperationException("Odometer cannot decrease.");
        }

        Odometer = miles;
    }

    public void RegisterTask(MaintenanceTask task)
    {
        if (task is null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (!Tasks.Contains(task))
        {
            Tasks.Add(task);
        }
    }
}
