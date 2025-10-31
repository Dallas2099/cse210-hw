namespace RiseWiseMoto.Domain;

public class RiderProfile
{
    public RiderProfile(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        Name = name;
    }

    public RiderProfile() : this("Unnamed Rider")
    {
    }

    public string Name { get; init; }

    public List<Motorcycle> Motorcycles { get; } = new();

    public int ExperiencePoints { get; private set; }

    public int CurrentLevel => 1 + ExperiencePoints / 100; // TODO: scale levels with streaks + badges later.

    public void AddXP(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "XP must be non-negative.");
        }

        ExperiencePoints += amount;
    }
}
