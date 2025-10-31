using Foundation2_Encapsulation.Domain;

Console.WriteLine("Foundation Program #2 — Encapsulation");
Console.WriteLine("----------------------------------------");

var bike = new Motorcycle(
    vin: "1HD1BJY10BB123456",
    make: "Harley-Davidson",
    model: "Iron 883",
    year: 2020,
    initialOdometer: 4_250,
    frontPsiTarget: 36,
    rearPsiTarget: 40);

Console.WriteLine(bike.Describe());
Console.WriteLine($"Front PSI target: {bike.FrontPsiTarget}");
Console.WriteLine($"Rear PSI target: {bike.RearPsiTarget}");

Console.WriteLine();
Console.WriteLine("Testing encapsulation safeguards...");

try
{
    bike.UpdateOdometer(4_500);
    Console.WriteLine($"Odometer advanced to {bike.Odometer:N0} miles");

    bike.UpdateOdometer(4_400); // should fail
}
catch (Exception ex)
{
    Console.WriteLine($"Blocked invalid odometer update: {ex.Message}");
}

try
{
    bike.SetPsiTargets(front: 8, rear: 42); // front too low
}
catch (Exception ex)
{
    Console.WriteLine($"Rejected tire pressure: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("State after attempted invalid changes:");
Console.WriteLine(bike.Describe());
Console.WriteLine($"Front PSI target: {bike.FrontPsiTarget}");
Console.WriteLine($"Rear PSI target: {bike.RearPsiTarget}");

Console.WriteLine();
Console.WriteLine("Setting new valid targets...");

bike.SetPsiTargets(front: 34, rear: 38);
Console.WriteLine($"Front PSI target: {bike.FrontPsiTarget}");
Console.WriteLine($"Rear PSI target: {bike.RearPsiTarget}");
