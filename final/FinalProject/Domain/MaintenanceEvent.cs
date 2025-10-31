namespace RiseWiseMoto.Domain;

public record MaintenanceEvent(string TaskName, DateOnly CompletedOn, int OdometerReading, string MotorcycleVin);
