using RiseWiseMoto.Domain;

namespace RiseWiseMoto.Services;

public class ReminderService
{
    public IEnumerable<ReminderResult> Scan(RiderProfile profile, DateOnly today)
    {
        if (profile is null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        foreach (var moto in profile.Motorcycles)
        {
            foreach (var task in moto.Tasks)
            {
                var status = task.IsDue(today, moto.Odometer)
                    ? "Due"
                    : task.IsDueSoon(today, moto.Odometer) ? "Soon" : "OK";

                yield return new ReminderResult(
                    TaskName: $"{moto.Make} {moto.Model}: {task.Name}",
                    Status: status,
                    NextDueHint: task.NextDueHint());
            }
        }
    }
}

public record ReminderResult(string TaskName, string Status, string NextDueHint);
