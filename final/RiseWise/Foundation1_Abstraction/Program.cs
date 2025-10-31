using Foundation1_Abstraction.Domain;

Console.WriteLine("Foundation Program #1 — Abstraction");
Console.WriteLine("----------------------------------------");

var today = new DateOnly(2024, 6, 1);
var currentOdometer = 7_800;

MaintenanceTask[] tasks =
{
    new MileageTask(
        id: "oil-change",
        name: "Engine Oil Change",
        intervalMiles: 3_000,
        lastDoneDate: new DateOnly(2024, 2, 15),
        lastDoneOdometer: 4_600),
    new TimeTask(
        id: "battery-check",
        name: "Battery Tender Check",
        intervalDays: 30,
        lastDoneDate: new DateOnly(2024, 5, 15),
        lastDoneOdometer: null)
};

foreach (var task in tasks)
{
    var isDue = task.IsDue(today, currentOdometer);
    var status = isDue ? "Due" : "Not Due";

    Console.WriteLine($"Task: {task.Name}");
    Console.WriteLine($"  Status: {status}");
    Console.WriteLine($"  Hint: {task.NextDueHint()}");
    Console.WriteLine();
}

// Demonstrate polymorphic behavior when the abstract base type is used.
var serviceVisit = new DateOnly(2024, 6, 1);
var finalOdometer = 8_050;

foreach (var task in tasks)
{
    if (task.IsDue(today, currentOdometer))
    {
        task.MarkCompleted(serviceVisit, finalOdometer);
    }
}

Console.WriteLine("After service visit:");
foreach (var task in tasks)
{
    Console.WriteLine($"  {task.Name} next -> {task.NextDueHint()}");
}
