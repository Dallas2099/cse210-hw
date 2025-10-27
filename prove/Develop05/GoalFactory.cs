using System;
using System.IO;

static class GoalFactory
{
    public static Goal Deserialize(string dataLine)
    {
        if (string.IsNullOrWhiteSpace(dataLine))
        {
            throw new InvalidDataException("Goal data was empty.");
        }

        string[] parts = dataLine.Split('|', StringSplitOptions.None);
        if (parts.Length == 0)
        {
            throw new InvalidDataException("Goal data was missing a type identifier.");
        }

        return parts[0] switch
        {
            "SimpleGoal" when parts.Length >= 5
                => new SimpleGoal(parts[1], parts[2], ParseInt(parts[3]), bool.Parse(parts[4])),
            "EternalGoal" when parts.Length >= 4
                => new EternalGoal(parts[1], parts[2], ParseInt(parts[3])),
            "ChecklistGoal" when parts.Length >= 7
                => new ChecklistGoal(
                    parts[1],
                    parts[2],
                    ParseInt(parts[3]),
                    ParseInt(parts[5]),
                    ParseInt(parts[6]),
                    ParseInt(parts[4])),
            _ => throw new InvalidDataException($"Unrecognised goal entry: {dataLine}")
        };
    }

    private static int ParseInt(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return result;
        }

        throw new InvalidDataException($"Invalid numeric value '{value}' in goal data.");
    }
}
