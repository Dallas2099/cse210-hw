using System.Linq;
using RiseWiseMoto.Domain;
using RiseWiseMoto.Services;
using RiseWiseMoto.Storage;

Console.WriteLine("RiseWise Moto");
Console.WriteLine("=============");

var today = DateOnly.FromDateTime(DateTime.Today);

var riderName = Prompt("Enter rider name (default: Every Rider): ");
var profile = new RiderProfile(string.IsNullOrWhiteSpace(riderName) ? "Every Rider" : riderName.Trim());

Console.WriteLine();
Console.WriteLine("=== Motorcycle Intake ===");
var make = Prompt("Make: ");
var model = Prompt("Model: ");
var year = ReadInt("Model year: ");
var currentMileage = ReadInt("Current odometer reading: ");
var vin = Prompt("VIN: ");

var moto = new Motorcycle(
    string.IsNullOrWhiteSpace(make) ? "Unknown" : make.Trim(),
    string.IsNullOrWhiteSpace(model) ? "Unknown" : model.Trim(),
    year,
    string.IsNullOrWhiteSpace(vin) ? $"VIN-{Guid.NewGuid():N}".ToUpperInvariant() : vin.Trim(),
    currentMileage);

profile.Motorcycles.Add(moto);

var oilChange = new MileageTask("Engine Oil Change", 3_000)
{
    LastCompletedOn = today.AddMonths(-4),
    LastOdometer = Math.Max(0, currentMileage - 3_000)
};

var batteryCheck = new TimeTask("Battery Tender Check", 30)
{
    LastCompletedOn = today.AddDays(-20)
};

var tclocs = new ChecklistTask("TCLOCS Safety Walk", 7)
{
    LastCompletedOn = today.AddDays(-3)
};

moto.RegisterTask(oilChange);
moto.RegisterTask(batteryCheck);
moto.RegisterTask(tclocs);

AddAdditionalMaintenanceItems(moto);

var reminderService = new ReminderService();
var xpService = new XPService();
var store = new JsonStore();

Console.WriteLine();
Console.WriteLine("Reminder snapshot:");
var reminders = reminderService.Scan(profile, today).ToList();
foreach (var reminder in reminders)
{
    Console.WriteLine($"- {reminder.TaskName}: {reminder.Status} ({reminder.NextDueHint})");
}

Console.WriteLine();
Console.Write("Did you complete an oil change today? (y/N): ");
var performedOilChange = Console.ReadLine();

if (!string.IsNullOrWhiteSpace(performedOilChange) &&
    performedOilChange.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
{
    var newMileage = ReadInt("Enter odometer reading post-service: ");
    moto.UpdateOdometer(newMileage);
    oilChange.RecordCompletion(today, moto.Odometer);
    var oilEvent = new MaintenanceEvent(oilChange.Name, today, moto.Odometer, moto.Vin);
    var xpGained = xpService.AwardFor(oilEvent, onTime: true);
    profile.AddXP(xpGained);

    Console.WriteLine();
    Console.WriteLine($"Recorded maintenance: {oilEvent.TaskName} at {oilEvent.OdometerReading:N0} miles");
    Console.WriteLine($"XP gained: {xpGained}");
    Console.WriteLine($"Current level: {profile.CurrentLevel}");
}

var refreshed = reminderService.Scan(profile, today).ToList();
Console.WriteLine();
Console.WriteLine("Updated reminders:");
foreach (var reminder in refreshed)
{
    Console.WriteLine($"- {reminder.TaskName}: {reminder.Status} ({reminder.NextDueHint})");
}

store.Save(profile);
Console.WriteLine();
Console.WriteLine($"Profile saved to: {store.ProfilePath}");

var reloaded = store.Load();
Console.WriteLine($"Reloaded profile for {reloaded.Name} with {reloaded.Motorcycles.Count} motorcycle(s).");

static void AddAdditionalMaintenanceItems(Motorcycle moto)
{
    while (AskToContinue())
    {
        Console.WriteLine();
        Console.WriteLine("Add Maintenance Item");
        var name = Prompt("Task name: ");
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "General Maintenance";
        }

        var typeChoice = Prompt("Choose type - [M]ileage, [T]ime, [C]hecklist, [R]eading: ");
        MaintenanceTask task = typeChoice.Trim().ToUpperInvariant() switch
        {
            "M" => CreateMileageTask(name),
            "T" => CreateTimeTask(name),
            "C" => CreateChecklistTask(name),
            "R" => CreateReadingTask(name),
            _ => CreateChecklistTask(name) // default to checklist if unknown input.
        };

        moto.RegisterTask(task);
        Console.WriteLine($"Added {task.GetType().Name} '{task.Name}'. Hint: {task.NextDueHint()}");
    }
}

static MaintenanceTask CreateMileageTask(string name)
{
    var interval = ReadInt("Mileage interval (miles): ");
    return new MileageTask(name, interval);
}

static MaintenanceTask CreateTimeTask(string name)
{
    var interval = ReadInt("Interval (days): ");
    return new TimeTask(name, interval);
}

static MaintenanceTask CreateChecklistTask(string name)
{
    var interval = ReadInt("Checklist reminder interval (days): ");
    return new ChecklistTask(name, interval);
}

static MaintenanceTask CreateReadingTask(string name)
{
    var threshold = ReadDouble("Reading threshold value: ");
    return new ReadingTask(name, threshold);
}

static bool AskToContinue()
{
    while (true)
    {
        Console.Write("Add another maintenance item or quit? (A/Q): ");
        var response = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(response))
        {
            continue;
        }

        var trimmed = response.Trim();
        if (trimmed.Equals("A", StringComparison.OrdinalIgnoreCase) ||
            trimmed.Equals("Add", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (trimmed.Equals("Q", StringComparison.OrdinalIgnoreCase) ||
            trimmed.Equals("Quit", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        Console.WriteLine("Please respond with 'A' to add another or 'Q' to quit.");
    }
}

static string Prompt(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? string.Empty;
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (int.TryParse(input, out var value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid number.");
    }
}

static double ReadDouble(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var input = Console.ReadLine();
        if (double.TryParse(input, out var value))
        {
            return value;
        }

        Console.WriteLine("Please enter a valid number (decimals allowed).");
    }
}
