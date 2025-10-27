using System.Collections.Generic;

class RecordEventResult
{
    private RecordEventResult(bool isSuccessful, int pointsEarned, bool levelUp, List<string> newAchievements, string message)
    {
        IsSuccessful = isSuccessful;
        PointsEarned = pointsEarned;
        LevelUp = levelUp;
        NewAchievements = newAchievements.AsReadOnly();
        Message = message;
    }

    public bool IsSuccessful { get; }
    public int PointsEarned { get; }
    public bool LevelUp { get; }
    public IReadOnlyList<string> NewAchievements { get; }
    public string Message { get; }

    public static RecordEventResult Failure(string message)
    {
        return new RecordEventResult(false, 0, false, new List<string>(), message);
    }

    public static RecordEventResult Success(int pointsEarned, bool levelUp, List<string> newAchievements, string message)
    {
        return new RecordEventResult(true, pointsEarned, levelUp, newAchievements, message);
    }
}
