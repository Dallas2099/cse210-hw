using System.Text.Json;
using System.Text.Json.Serialization;
using RiseWiseMoto.Domain;

namespace RiseWiseMoto.Storage;

public class JsonStore
{
    private readonly string _dataDirectory;
    private readonly JsonSerializerOptions _options;

    public JsonStore(string? dataDirectory = null)
    {
        _dataDirectory = dataDirectory ?? Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(_dataDirectory);

        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
        _options.Converters.Add(new MaintenanceTaskConverter());
    }

    public string ProfilePath => Path.Combine(_dataDirectory, "profile.json");

    public void Save(RiderProfile profile)
    {
        if (profile is null)
        {
            throw new ArgumentNullException(nameof(profile));
        }

        var json = JsonSerializer.Serialize(profile, _options);
        File.WriteAllText(ProfilePath, json);
    }

    public RiderProfile Load()
    {
        if (!File.Exists(ProfilePath))
        {
            throw new FileNotFoundException("Profile file not found.", ProfilePath);
        }

        var json = File.ReadAllText(ProfilePath);
        return JsonSerializer.Deserialize<RiderProfile>(json, _options)
            ?? throw new InvalidOperationException("Failed to load rider profile from storage.");
    }

    private sealed class MaintenanceTaskConverter : JsonConverter<MaintenanceTask>
    {
        public override MaintenanceTask? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;

            if (!root.TryGetProperty("$type", out var typeElement))
            {
                throw new JsonException("Maintenance task is missing $type discriminator.");
            }

            var typeKey = typeElement.GetString() ?? throw new JsonException("Task discriminator is null.");

            MaintenanceTask task = typeKey switch
            {
                "mileage" => new MileageTask(),
                "time" => new TimeTask(),
                "reading" => new ReadingTask(),
                "checklist" => new ChecklistTask(),
                _ => throw new JsonException($"Unsupported maintenance task type '{typeKey}'.")
            };

            if (root.TryGetProperty("Name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
            {
                task.Name = nameElement.GetString() ?? string.Empty;
            }

            if (root.TryGetProperty("LastCompletedOn", out var dateElement) && dateElement.ValueKind == JsonValueKind.String)
            {
                if (DateOnly.TryParse(dateElement.GetString(), out var parsedDate))
                {
                    task.LastCompletedOn = parsedDate;
                }
            }

            if (root.TryGetProperty("LastOdometer", out var odoElement) && odoElement.ValueKind == JsonValueKind.Number)
            {
                task.LastOdometer = odoElement.GetInt32();
            }

            switch (task)
            {
                case MileageTask mileage when root.TryGetProperty("IntervalMiles", out var milesElement) && milesElement.ValueKind == JsonValueKind.Number:
                    mileage.IntervalMiles = milesElement.GetInt32();
                    break;
                case TimeTask time when root.TryGetProperty("IntervalDays", out var daysElement) && daysElement.ValueKind == JsonValueKind.Number:
                    time.IntervalDays = daysElement.GetInt32();
                    break;
                case ChecklistTask checklist when root.TryGetProperty("IntervalDays", out var checklistDays) && checklistDays.ValueKind == JsonValueKind.Number:
                    checklist.IntervalDays = checklistDays.GetInt32();
                    break;
                case ReadingTask reading:
                {
                    if (root.TryGetProperty("Threshold", out var thresholdElement) && thresholdElement.ValueKind == JsonValueKind.Number)
                    {
                        reading.Threshold = thresholdElement.GetDouble();
                    }

                    if (root.TryGetProperty("LastReading", out var lastReadingElement))
                    {
                        reading.LastReading = lastReadingElement.ValueKind switch
                        {
                            JsonValueKind.Number => lastReadingElement.GetDouble(),
                            _ => null
                        };
                    }

                    break;
                }
            }

            return task;
        }

        public override void Write(Utf8JsonWriter writer, MaintenanceTask value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("$type", value switch
            {
                MileageTask => "mileage",
                TimeTask => "time",
                ReadingTask => "reading",
                ChecklistTask => "checklist",
                _ => throw new JsonException($"Unsupported maintenance task type '{value.GetType().Name}'.")
            });

            writer.WriteString("Name", value.Name);

            if (value.LastCompletedOn is { } date)
            {
                writer.WriteString("LastCompletedOn", date.ToString("yyyy-MM-dd"));
            }

            if (value.LastOdometer is { } odometer)
            {
                writer.WriteNumber("LastOdometer", odometer);
            }

            switch (value)
            {
                case MileageTask mileage:
                    writer.WriteNumber("IntervalMiles", mileage.IntervalMiles);
                    break;
                case TimeTask time:
                    writer.WriteNumber("IntervalDays", time.IntervalDays);
                    break;
                case ChecklistTask checklist:
                    writer.WriteNumber("IntervalDays", checklist.IntervalDays);
                    break;
                case ReadingTask reading:
                    writer.WriteNumber("Threshold", reading.Threshold);
                    if (reading.LastReading is { } lastReading)
                    {
                        writer.WriteNumber("LastReading", lastReading);
                    }

                    break;
            }

            writer.WriteEndObject();
        }
    }
}
