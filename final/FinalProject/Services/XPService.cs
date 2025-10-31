using RiseWiseMoto.Domain;

namespace RiseWiseMoto.Services;

public class XPService
{
    public int AwardFor(MaintenanceEvent maintenanceEvent, bool onTime)
    {
        if (maintenanceEvent is null)
        {
            throw new ArgumentNullException(nameof(maintenanceEvent));
        }

        var award = onTime ? 25 : 15;

        if (maintenanceEvent.TaskName.Contains("oil", StringComparison.OrdinalIgnoreCase))
        {
            award += 5;
        }

        return award; // TODO: scale XP using rider streaks and motorcycle difficulty rating.
    }
}
