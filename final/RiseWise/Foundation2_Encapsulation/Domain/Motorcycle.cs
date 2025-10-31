namespace Foundation2_Encapsulation.Domain;

/// <summary>
/// A simple motorcycle entity that protects against invalid state.
/// </summary>
public class Motorcycle
{
    private int _odometer;
    private int _frontPsiTarget;
    private int _rearPsiTarget;

    public Motorcycle(string vin, string make, string model, int year, int initialOdometer, int frontPsiTarget, int rearPsiTarget)
    {
        if (string.IsNullOrWhiteSpace(vin))
        {
            throw new ArgumentException("VIN is required.", nameof(vin));
        }

        if (year < 1950 || year > DateTime.UtcNow.Year + 1)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Year is out of realistic range.");
        }

        Vin = vin;
        Make = make;
        Model = model;
        Year = year;

        UpdateOdometer(initialOdometer);
        SetPsiTargets(frontPsiTarget, rearPsiTarget);
    }

    public string Vin { get; }
    public string Make { get; }
    public string Model { get; }
    public int Year { get; }

    public int Odometer => _odometer;
    public int FrontPsiTarget => _frontPsiTarget;
    public int RearPsiTarget => _rearPsiTarget;

    public void UpdateOdometer(int miles)
    {
        if (miles < _odometer)
        {
            throw new InvalidOperationException("Odometer cannot roll back.");
        }

        _odometer = miles;
    }

    public void SetPsiTargets(int front, int rear)
    {
        ValidatePsi(front, nameof(front));
        ValidatePsi(rear, nameof(rear));

        _frontPsiTarget = front;
        _rearPsiTarget = rear;
    }

    public string Describe() => $"{Year} {Make} {Model} ({Vin}) - {Odometer:N0} miles";

    private static void ValidatePsi(int psi, string argumentName)
    {
        if (psi is < 10 or > 50)
        {
            throw new ArgumentOutOfRangeException(argumentName, "PSI targets must stay between 10 and 50.");
        }
    }
}
